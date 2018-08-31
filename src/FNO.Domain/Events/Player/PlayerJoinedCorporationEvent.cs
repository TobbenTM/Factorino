using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerJoinedCorporationEvent : EntityEvent
    {
        public Guid CorporationId { get; set; }
        public Guid? InvitationId { get; set; }
        
        public PlayerJoinedCorporationEvent(Guid playerId, Guid corporationId, Guid? invitationId = null) : base(playerId)
        {
            CorporationId = corporationId;
            InvitationId = invitationId;
        }
    }
}
