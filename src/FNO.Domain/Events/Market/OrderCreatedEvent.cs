using System;
using FNO.Domain.Models.Market;

namespace FNO.Domain.Events.Market
{
    public class OrderCreatedEvent : EntityEvent
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public OrderType OrderType { get; set; }

        public OrderCreatedEvent()
        {
        }

        public OrderCreatedEvent(Guid orderId, Models.Player initiator) : base(orderId, initiator)
        {
        }
    }
}
