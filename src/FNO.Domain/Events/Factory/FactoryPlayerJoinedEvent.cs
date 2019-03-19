using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerJoinedEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerJoinedEvent()
        {
        }

        public FactoryPlayerJoinedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }

        public override string ReadableEvent => $"{PlayerName} joined the server";
    }
}
