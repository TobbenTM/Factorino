using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerInvitedToCorporationEvent : EntityEvent
    {
        public Guid CorporationId { get; set; }
        public Guid InvitationId { get; set; }

        public PlayerInvitedToCorporationEvent()
        {
        }

        public PlayerInvitedToCorporationEvent(Guid playerId, Guid corporationId, Guid invitationId, Models.Player initiator) : base(playerId, initiator)
        {
            CorporationId = corporationId;
            InvitationId = invitationId;
        }
    }
}
