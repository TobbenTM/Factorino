﻿using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryBuiltEntityEvent : FactoryActivityBaseEvent
    {
        public FactoryBuiltEntityEvent()
        {
        }

        public FactoryBuiltEntityEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }
        public LuaEntity Entity { get; set; }

        public override string ReadableEvent => $"{PlayerName} built a {Entity.Name}";
    }
}
