using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Market;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.EventSourcing;

namespace FNO.Broker.EventHandlers
{
    public class OrderEventHandler : IHandler,
        IEventHandler<OrderCancelledEvent>,
        IEventHandler<OrderCreatedEvent>,
        IEventHandler<OrderFulfilledEvent>,
        IEventHandler<OrderTransactionEvent>
    {
        private readonly State _state;

        public OrderEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(OrderCreatedEvent evnt)
        {
            var player = _state.Players[evnt.OwnerId];
            var order = new BrokerOrder
            {
                OrderId = evnt.EntityId,
                OwnerId = evnt.OwnerId,
                ItemId = evnt.ItemId,
                Quantity = evnt.Quantity,
                Price = evnt.Price,
                OrderType = evnt.OrderType,
                State = OrderState.Active,
                Owner = player,
            };
            _state.Orders.Add(evnt.EntityId, order);
            _state.OrdersByItemId.Add(evnt.ItemId, order);
            return Task.CompletedTask;
        }

        public Task Handle(OrderFulfilledEvent evnt)
        {
            var order = _state.Orders[evnt.EntityId];
            order.State = OrderState.Fulfilled;
            return Task.CompletedTask;
        }

        public Task Handle(OrderCancelledEvent evnt)
        {
            var order = _state.Orders[evnt.EntityId];
            order.State = OrderState.Cancelled;
            return Task.CompletedTask;
        }

        public Task Handle(OrderTransactionEvent evnt)
        {
            _state.HandledTransactions.TryPeek(out var handledTransaction);
            if (handledTransaction == evnt.EntityId)
            {
                // If we've already handled this event through the evaluator,
                // we don't want to mess with credits and inventory
                _state.HandledTransactions.Dequeue();
                return Task.CompletedTask;
            }

            var fromOrder = _state.Orders[evnt.FromSellOrder];
            var toOrder = _state.Orders[evnt.ToBuyOrder];

            fromOrder.QuantityFulfilled += evnt.Quantity;
            toOrder.QuantityFulfilled += evnt.Quantity;

            fromOrder.Owner.Credits += evnt.Price;
            toOrder.Owner.Credits -= evnt.Price;

            fromOrder.Owner.Inventory[evnt.ItemId].Quantity -= evnt.Quantity;

            if (toOrder.Owner.Inventory.TryGetValue(evnt.ItemId, out var inventory))
            {
                inventory.Quantity += evnt.Quantity;
            }
            else
            {
                toOrder.Owner.Inventory.Add(evnt.ItemId, new WarehouseInventory
                {
                    ItemId = evnt.ItemId,
                    Quantity = evnt.Quantity,
                });
            }

            return Task.CompletedTask;
        }
    }
}
