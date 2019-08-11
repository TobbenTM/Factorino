using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using FNO.Orchestrator.Models;
using Serilog;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    public class EventHandler :
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryProvisionedEvent>,
        IEventHandler<FactoryDestroyedEvent>,
        IEventHandler<FactoryDecommissionedEvent>
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
                OwnerId = evnt.Initiator.PlayerId,
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
            factory.ResourceId = evnt.ResourceId;
            return Task.CompletedTask;
        }

        public Task Handle(FactoryDestroyedEvent evnt)
        {
            var factory = _state.GetFactory(evnt.EntityId);
            factory.State = FactoryState.Destroying;
            return Task.CompletedTask;
        }

        public Task Handle(FactoryDecommissionedEvent evnt)
        {
            var factory = _state.GetFactory(evnt.EntityId);
            factory.State = FactoryState.Destroyed;
            return Task.CompletedTask;
        }
    }
}
