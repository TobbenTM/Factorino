using System;
using FNO.Domain.Models.Market;

namespace FNO.Domain.Events.Market
{
    public class OrderCreatedEvent : EntityEvent
    {
        public Guid OwnerId { get; set; }

        public string ItemId { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public OrderType OrderType { get; set; }

        public OrderCreatedEvent()
        {
        }

        public OrderCreatedEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
            OwnerId = initiator.PlayerId;
        }
    }
}
