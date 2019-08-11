using System;

namespace FNO.Domain.Events.Market
{
    public class OrderTransactionEvent : EntityEvent
    {
        public Guid FromSellOrder { get; set; }
        public Guid FromPlayer { get; set; }
        public Guid ToBuyOrder { get; set; }
        public Guid ToPlayer { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public string ItemId { get; set; }

        public OrderTransactionEvent()
        {
        }

        public OrderTransactionEvent(Guid transactionId, Models.Player initiator) : base(transactionId, initiator)
        {

        }
    }
}
