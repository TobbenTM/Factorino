using System;

namespace FNO.Domain.Events
{
    public abstract class EntityEvent : Event, IEntityEvent
    {
        public Guid EntityId { get; set; }

        public EntityEvent()
        {
        }

        public EntityEvent(Guid entityId)
        {
            EntityId = entityId;
        }
    }
}
