using System;

namespace FNO.Domain.Events
{
    public abstract class EntityEvent : Event, IEntityEvent
    {
        public Guid EntityId { get; set; }

        protected EntityEvent()
        {
        }

        protected EntityEvent(Guid entityId, Models.Player initiator) : base(initiator)
        {
            EntityId = entityId;
        }
    }
}
