using System.Linq;
using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events.Market;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FNO.ReadModel.EventHandlers
{
    public class MarketEventHandler : EventHandlerBase,
        IEventHandler<OrderCreatedEvent>,
        IEventHandler<OrderPartiallyFulfilledEvent>,
        IEventHandler<OrderFulfilledEvent>,
        IEventHandler<OrderCancelledEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public MarketEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(OrderCreatedEvent evnt)
        {
            _dbContext.Orders.Add(new MarketOrder
            {
                OrderId = evnt.EntityId,
                OwnerId = evnt.OwnerId,
                ItemId = evnt.ItemId,
                Quantity = evnt.Quantity,
                Price = evnt.Price,
                OrderType = evnt.OrderType,
                State = OrderState.Active,
            });
            return Task.CompletedTask;
        }

        public Task Handle(OrderPartiallyFulfilledEvent evnt)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == evnt.EntityId);
            if (order != null)
            {
                order.QuantityFulfilled += evnt.QuantityFulfilled;
                order.State = OrderState.PartiallyFulfilled;
            }
            return Task.CompletedTask;
        }

        public Task Handle(OrderFulfilledEvent evnt)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == evnt.EntityId);
            if (order != null)
            {
                order.QuantityFulfilled = order.Quantity;
                order.State = OrderState.Fulfilled;
            }
            return Task.CompletedTask;
        }

        public Task Handle(OrderCancelledEvent evnt)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == evnt.EntityId);
            if (order != null)
            {
                order.CancellationReason = evnt.CancellationReason;
                order.State = OrderState.Cancelled;
            }
            return Task.CompletedTask;
        }
    }
}
