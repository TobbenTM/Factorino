using FNO.Domain.Models.Shipping;
using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentRequestedEvent : EntityEvent
    {
        public Guid FactoryId { get; set; }
        public Guid OwnerId { get; set; }

        public Cart[] Carts { get; set; }
        public string DestinationStation { get; set; }
        public WaitCondition[] WaitConditions { get; set; }

        public ShipmentRequestedEvent()
        {
        }

        public ShipmentRequestedEvent(Guid entityId, Guid factoryId, Models.Player initiator) : base(entityId, initiator)
        {
            FactoryId = factoryId;
            OwnerId = initiator.PlayerId;
        }
    }
}
