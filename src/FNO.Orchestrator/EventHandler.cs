using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Serilog;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    public class EventHandler : IEventHandler<FactoryCreatedEvent>, IEventHandler<FactoryProvisionedEvent>
    {
        private readonly State _state;
        private readonly ILogger _logger;

        public EventHandler(State state, ILogger logger)
        {
            _state = state;
            _logger = logger;
        }

        public Task Handle(FactoryCreatedEvent evnt)
        {
            _state.AddFactory(new Factory
            {
                FactoryId = evnt.EntityId,
                State = FactoryState.Creating,
                Seed = evnt.LocationSeed,
                LocationId = evnt.LocationId,
            });
            return Task.CompletedTask;
        }

        public Task Handle(FactoryProvisionedEvent evnt)
        {
            var factory = _state.GetFactory(evnt.EntityId);
            factory.State = FactoryState.Starting;
            return Task.CompletedTask;
        }
    }
}
