﻿using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerDiedEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerDiedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }
    }
}
