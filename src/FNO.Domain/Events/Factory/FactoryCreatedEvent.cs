using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryCreatedEvent : EntityEvent
    {
        public FactoryCreatedEvent()
        {
        }

        public FactoryCreatedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }
    }
}
