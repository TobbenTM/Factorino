using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryDecommissionedEvent : EntityEvent
    {
        public FactoryDecommissionedEvent()
        {
        }

        public FactoryDecommissionedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }
    }
}
