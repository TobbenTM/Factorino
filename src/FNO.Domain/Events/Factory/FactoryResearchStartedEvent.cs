using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryResearchStartedEvent : FactoryActivityBaseEvent
    {
        public FactoryResearchStartedEvent()
        {
        }

        public FactoryResearchStartedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public LuaTechnology Technology { get; set; }

        public override string ReadableEvent => $"Factory started researching {Technology.Name}";
    }
}
