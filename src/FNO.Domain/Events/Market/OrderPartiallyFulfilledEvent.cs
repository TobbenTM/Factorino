using System;

namespace FNO.Domain.Events.Market
{
    public class OrderPartiallyFulfilledEvent : EntityEvent
    {
        public OrderPartiallyFulfilledEvent()
        {
        }

        public OrderPartiallyFulfilledEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
