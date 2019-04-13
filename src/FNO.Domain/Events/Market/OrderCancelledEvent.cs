using System;
using FNO.Domain.Models.Market;

namespace FNO.Domain.Events.Market
{
    public class OrderCancelledEvent : EntityEvent
    {
        public OrderCancellationReason CancellationReason { get; set; }

        public OrderCancelledEvent()
        {
        }

        public OrderCancelledEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
