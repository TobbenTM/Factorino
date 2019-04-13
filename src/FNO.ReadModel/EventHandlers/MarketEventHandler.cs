using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events.Market;
using FNO.EventSourcing;
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
            throw new System.NotImplementedException();
        }

        public Task Handle(OrderPartiallyFulfilledEvent evnt)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(OrderFulfilledEvent evnt)
        {
            throw new System.NotImplementedException();
        }

        public Task Handle(OrderCancelledEvent evnt)
        {
            throw new System.NotImplementedException();
        }
    }
}
