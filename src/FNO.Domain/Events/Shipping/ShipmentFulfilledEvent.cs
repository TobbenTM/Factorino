using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentFulfilledEvent : EntityEvent
    {
        public Guid FactoryId { get; set; }
        public Guid OwnerId { get; set; }

        public ShipmentFulfilledEvent()
        {
        }

        public ShipmentFulfilledEvent(Guid shipmentId, Guid factoryId, Models.Player initiator) : base(shipmentId, initiator)
        {
            FactoryId = factoryId;
        }
    }
}
