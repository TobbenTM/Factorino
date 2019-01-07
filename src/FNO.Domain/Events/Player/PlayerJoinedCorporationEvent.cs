using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerJoinedCorporationEvent : EntityEvent
    {
        public Guid CorporationId { get; set; }
        public Guid? InvitationId { get; set; }

        public PlayerJoinedCorporationEvent()
        {
        }

        public PlayerJoinedCorporationEvent(Guid playerId, Guid corporationId, Models.Player initiator, Guid? invitationId = null) : base(playerId, initiator)
        {
            CorporationId = corporationId;
            InvitationId = invitationId;
        }
    }
}
