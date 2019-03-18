using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerLeftEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerLeftEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }

        public override string ReadableEvent => $"{PlayerName} left the server";
    }
}
