using FNO.Domain;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel.EventHandlers
{
    public sealed class FactoryEventHandler : EventHandlerBase,
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryOnlineEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(FactoryOnlineEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Online;
                factory.LastSeen = evnt.Metadata.CreatedAt ?? factory.LastSeen;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryCreatedEvent evnt)
        {
            _dbContext.Factories.Add(new Factory
            {
                FactoryId = evnt.EntityId,
                OwnerId = evnt.Initiator.PlayerId,
                State = FactoryState.Creating,
            });
            return Task.CompletedTask;
        }
    }
}
