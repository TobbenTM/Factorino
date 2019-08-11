using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;
using FNO.EventSourcing;

namespace FNO.Broker.EventHandlers
{
    public class ShipmentEventHandler : IHandler,
        IEventHandler<ShipmentRequestedEvent>,
        IEventHandler<ShipmentFulfilledEvent>,
        IEventHandler<ShipmentCompletedEvent>
    {
        private readonly State _state;

        public ShipmentEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(ShipmentRequestedEvent evnt)
        {
            var player = _state.Players[evnt.OwnerId];
            _state.Shipments.Add(evnt.EntityId, new BrokerShipment
            {
                ShipmentId = evnt.EntityId,
                FactoryId = evnt.FactoryId,
                Owner = player,
                State = ShipmentState.Requested,
                Carts = evnt.Carts,
                DestinationStation = evnt.DestinationStation,
                WaitConditions = evnt.WaitConditions,
            });
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentFulfilledEvent evnt)
        {
            _state.HandledShipments.TryPeek(out var handledShipment);
            if (handledShipment == evnt.EntityId)
            {
                // If we've already handled this event through the evaluator,
                // we don't want to mess with inventory
                _state.HandledShipments.Dequeue();
                return Task.CompletedTask;
            }

            var shipment = _state.Shipments[evnt.EntityId];

            foreach (var cart in shipment.Carts)
            {
                foreach (var stack in cart.Inventory)
                {
                    shipment.Owner.Inventory[stack.Name].Quantity -= stack.Count;
                }
            }

            shipment.State = ShipmentState.Fulfilled;
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentCompletedEvent evnt)
        {
            var shipment = _state.Shipments[evnt.EntityId];

            foreach (var stack in evnt.ReturningCargo)
            {
                if (shipment.Owner.Inventory.TryGetValue(stack.Name, out var inventory))
                {
                    inventory.Quantity += stack.Count;
                }
                else
                {
                    shipment.Owner.Inventory.Add(stack.Name, new WarehouseInventory
                    {
                        ItemId = stack.Name,
                        Quantity = stack.Count,
                    });
                }
            }

            shipment.State = ShipmentState.Completed;
            return Task.CompletedTask;
        }
    }
}
