using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerRejectedInvitationEvent : EntityEvent
    {
        public Guid InvitationId { get; set; }

        public PlayerRejectedInvitationEvent(Guid playerId, Guid invitationId) : base(playerId)
        {
            InvitationId = invitationId;
        }
    }
}
