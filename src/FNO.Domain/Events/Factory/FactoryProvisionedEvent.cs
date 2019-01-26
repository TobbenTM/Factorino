using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryProvisionedEvent : EntityEvent
    {
        public FactoryProvisionedEvent()
        {
        }

        public FactoryProvisionedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }
    }
}
