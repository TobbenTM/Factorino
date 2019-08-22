using System;

namespace FNO.Domain.Events.Market
{
    public class OrderPartiallyFulfilledEvent : EntityEvent
    {
        public long QuantityFulfilled { get; set; }

        public long Price { get; set; }

        public Guid TransactionId { get; set; }

        public OrderPartiallyFulfilledEvent()
        {
        }

        public OrderPartiallyFulfilledEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
