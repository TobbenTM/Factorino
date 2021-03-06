﻿using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryChatEvent : FactoryActivityBaseEvent
    {
        public FactoryChatEvent()
        {
        }

        public FactoryChatEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }

        public string Message { get; set; }

        public override string ReadableEvent => $"{PlayerName} said \"{Message}\"";
    }
}
