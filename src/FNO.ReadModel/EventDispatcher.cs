using FNO.Domain;
using FNO.Domain.Events;
using FNO.Domain.Events.Corporation;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Player;
using FNO.EventSourcing;
using FNO.ReadModel.EventHandlers;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly EventHandlerResolver _resolver;
        private readonly ReadModelDbContext _dbContext;
        private readonly ILogger _logger;

        public EventDispatcher(ReadModelDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _resolver = new EventHandlerResolver();

            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            _resolver.Register(() => new PlayerEventHandler(_dbContext, _logger),
                typeof(PlayerCreatedEvent),
                typeof(PlayerInvitedToCorporationEvent),
                typeof(PlayerJoinedCorporationEvent),
                typeof(PlayerLeftCorporationEvent),
                typeof(PlayerRejectedInvitationEvent));

            _resolver.Register(() => new CorporationEventHandler(_dbContext, _logger),
                typeof(CorporationCreatedEvent));

            _resolver.Register(() => new FactoryEventHandler(_dbContext, _logger),
                typeof(FactoryCreatedEvent),
                typeof(FactoryProvisionedEvent),
                typeof(FactoryOnlineEvent));
        }

        public async Task Handle<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var handlers = _resolver.Resolve(evnt);
            if (!handlers.Any())
            {
                _logger.Information($"Skipping event of type {evnt.GetType().FullName}, no handlers registered.");
                return;
            }
            foreach (var handler in handlers)
            {
                await handler.Handle(evnt);
            }
        }
    }
}
