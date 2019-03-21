using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Shipping
{
    public class ShipmentCompletedEvent : EntityEvent
    {
        public Guid FactoryId { get; set; }
        public Guid OwnerId { get; set; }
        public LuaItemStack[] ReturningCargo { get; set; }

        public ShipmentCompletedEvent()
        {
        }

        public ShipmentCompletedEvent(Guid entityId, Guid factoryId, Models.Player initiator) : base(entityId, initiator)
        {
            FactoryId = factoryId;
        }
    }
}
