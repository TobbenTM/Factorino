using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerJoinedCorporationEvent : IEntityEvent
    {
        /// <summary>
        /// Player Id
        /// </summary>
        public Guid EntityId { get; set; }

        public Guid CorporationId { get; set; }
    }
}
