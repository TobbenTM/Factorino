using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentReceivedEvent : EntityEvent
    {
        public Guid FactoryId { get; set; }
        public Guid OwnerId { get; set; }

        public ShipmentReceivedEvent()
        {
        }

        public ShipmentReceivedEvent(Guid entityId, Guid factoryId, Models.Player initiator) : base(entityId, initiator)
        {
            FactoryId = factoryId;
        }
    }
}
