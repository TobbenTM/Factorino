using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using FNO.Orchestrator.Models;
using System.Threading.Tasks;

namespace FNO.Orchestrator.EventHandlers
{
    public class FactoryEventHandler : IHandler,
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryProvisionedEvent>,
        IEventHandler<FactoryDestroyedEvent>,
        IEventHandler<FactoryDecommissionedEvent>
    {
        private readonly State _state;

        public FactoryEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(FactoryCreatedEvent evnt)
        {
            _state.AddFactory(new Factory
            {
                FactoryId = evnt.EntityId,
                OwnerId = evnt.Initiator.PlayerId,
                OwnerFactorioUsername = _state.GetUsername(evnt.Initiator.PlayerId),
                State = FactoryState.Creating,
                DeedId = evnt.DeedId,
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
