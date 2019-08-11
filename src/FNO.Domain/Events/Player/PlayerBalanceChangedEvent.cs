﻿using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerBalanceChangedEvent : EntityEvent
    {
        public int BalanceChange { get; set; }

        public PlayerBalanceChangedEvent()
        {
        }

        public PlayerBalanceChangedEvent(Guid playerId, Models.Player initiator) : base(playerId, initiator)
        {
        }
    }
}