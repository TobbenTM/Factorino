using System;

namespace FNO.Domain.Events.Market
{
    public class OrderTransactionEvent : Event
    {
        public Guid FromSellOrder { get; set; }
        public Guid ToBuyOrder { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public OrderTransactionEvent()
        {
        }

        public OrderTransactionEvent(Models.Player initiator) : base(initiator)
        {

        }
    }
}
