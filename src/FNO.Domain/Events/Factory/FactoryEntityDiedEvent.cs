using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryEntityDiedEvent : FactoryActivityBaseEvent
    {
        public FactoryEntityDiedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }
        public LuaEntity Entity { get; set; }

        public override string ReadableEvent => $"{PlayerName} killed a {Entity.Name}";
    }
}
