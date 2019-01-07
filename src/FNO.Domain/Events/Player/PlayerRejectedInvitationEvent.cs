using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerRejectedInvitationEvent : EntityEvent
    {
        public Guid InvitationId { get; set; }

        public PlayerRejectedInvitationEvent()
        {
        }

        public PlayerRejectedInvitationEvent(Guid playerId, Guid invitationId, Models.Player initiator) : base(playerId, initiator)
        {
            InvitationId = invitationId;
        }
    }
}
