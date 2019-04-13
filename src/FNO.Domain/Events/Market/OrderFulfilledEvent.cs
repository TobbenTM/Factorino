using System;

namespace FNO.Domain.Events.Market
{
    public class OrderFulfilledEvent : EntityEvent
    {
        public OrderFulfilledEvent()
        {
        }

        public OrderFulfilledEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
