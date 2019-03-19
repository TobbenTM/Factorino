using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryResearchFinishedEvent : FactoryActivityBaseEvent
    {
        public FactoryResearchFinishedEvent()
        {
        }

        public FactoryResearchFinishedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public LuaTechnology Technology { get; set; }

        public override string ReadableEvent => $"Factory finished researching {Technology.Name}";
    }
}
