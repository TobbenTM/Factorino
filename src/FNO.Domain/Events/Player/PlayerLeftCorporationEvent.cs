using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerLeftCorporationEvent : EntityEvent
    {
        public Guid CorporationId { get; set; }

        public PlayerLeftCorporationEvent()
        {
        }

        public PlayerLeftCorporationEvent(Guid playerId, Guid corporationId) : base(playerId)
        {
            CorporationId = corporationId;
        }
    }
}
