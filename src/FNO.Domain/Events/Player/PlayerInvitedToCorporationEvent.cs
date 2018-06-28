using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerInvitedToCorporationEvent : IEntityEvent
    {
        /// <summary>
        /// Player Id
        /// </summary>
        public Guid EntityId { get; set; }

        public Guid CorporationId { get; set; }
    }
}
