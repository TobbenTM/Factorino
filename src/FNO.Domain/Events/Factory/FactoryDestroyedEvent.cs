using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryDestroyedEvent : EntityEvent
    {
        public FactoryDestroyedEvent()
        {
        }

        public FactoryDestroyedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }
    }
}
