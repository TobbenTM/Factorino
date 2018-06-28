using System;

namespace FNO.Domain.Events
{
    public interface IEntityEvent : IEvent
    {
        Guid EntityId { get; set; }
    }
}
