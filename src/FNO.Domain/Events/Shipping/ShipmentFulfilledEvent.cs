using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentFulfilledEvent : EntityEvent
    {
        public ShipmentFulfilledEvent()
        {
        }

        public ShipmentFulfilledEvent(Guid entityId, Models.Player initiator) : base(entityId, initiator)
        {
        }
    }
}
