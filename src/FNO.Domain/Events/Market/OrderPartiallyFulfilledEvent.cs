using System;

namespace FNO.Domain.Events.Market
{
    public class OrderPartiallyFulfilledEvent : EntityEvent
    {
        public int QuantityFulfilled { get; set; }
        public int Price { get; set; }

        public OrderPartiallyFulfilledEvent()
        {
        }

        public OrderPartiallyFulfilledEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
