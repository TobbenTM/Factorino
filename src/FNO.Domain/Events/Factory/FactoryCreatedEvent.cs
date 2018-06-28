using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryCreatedEvent : EntityEvent
    {
        public FactoryCreatedEvent(Guid factoryId) : base(factoryId)
        {
        }
    }
}
