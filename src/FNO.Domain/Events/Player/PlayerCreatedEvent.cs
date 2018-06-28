using System;

namespace FNO.Domain.Events.Player
{
    class PlayerCreatedEvent : EntityEvent
    {
        public PlayerCreatedEvent(Guid playerId) : base(playerId)
        {
        }
    }
}
