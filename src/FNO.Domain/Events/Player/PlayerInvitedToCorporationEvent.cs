﻿using System;
using System.Diagnostics;

namespace FNO.Domain.Events.Player
{
    public class PlayerInvitedToCorporationEvent : EntityEvent
    {
        public Guid CorporationId { get; set; }
        public Guid InvitationId { get; set; }

        public PlayerInvitedToCorporationEvent()
        {
        }

        public PlayerInvitedToCorporationEvent(Guid playerId, Guid corporationId, Guid invitationId) : base(playerId)
        {
            CorporationId = corporationId;
            InvitationId = invitationId;
        }
    }
}
