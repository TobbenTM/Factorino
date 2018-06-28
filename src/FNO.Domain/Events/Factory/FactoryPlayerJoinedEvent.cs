using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerJoinedEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerJoinedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }
    }
}
