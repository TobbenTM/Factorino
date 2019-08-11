using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;

namespace FNO.Broker.EventHandlers
{
    public class FactoryEventHandler : IHandler,
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryOutgoingTrainEvent>
    {
        private readonly State _state;

        public FactoryEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(FactoryCreatedEvent evnt)
        {
            var owner = _state.Players[evnt.Initiator.PlayerId];
            _state.FactoryOwners.Add(evnt.EntityId, owner);
            return Task.CompletedTask;
        }

        public Task Handle(FactoryOutgoingTrainEvent evnt)
        {
            var player = _state.FactoryOwners[evnt.EntityId];
            foreach (var stack in evnt.Inventory)
            {
                if (player.Inventory.TryGetValue(stack.Name, out var inventory))
                {
                    inventory.Quantity += stack.Count;
                }
                else
                {
                    player.Inventory.Add(stack.Name, new WarehouseInventory
                    {
                        ItemId = stack.Name,
                        Quantity = stack.Count,
                    });
                }
            }
            return Task.CompletedTask;
        }
    }
}
