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

        public ShipmentRequestedEvent(Guid shipmentId, Guid factoryId, Models.Player initiator) : base(shipmentId, initiator)
        {
            FactoryId = factoryId;
            OwnerId = initiator.PlayerId;
        }
    }
}
