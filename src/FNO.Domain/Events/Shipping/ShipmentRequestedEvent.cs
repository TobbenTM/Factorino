using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentRequestedEvent : EntityEvent
    {
        public ShipmentRequestedEvent()
        {
        }

        public ShipmentRequestedEvent(Guid entityId, Models.Player initiator) : base(entityId, initiator)
        {
        }
    }
}
