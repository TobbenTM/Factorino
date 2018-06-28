using System;

namespace FNO.Domain.Events.Factory
{
    public abstract class FactoryActivityBaseEvent : EntityEvent
    {
        public FactoryActivityBaseEvent(Guid factoryId, string type, long tick) : base(factoryId)
        {
            Type = type;
            Tick = tick;
        }

        /// <summary>
        /// Type of native event
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Tick of the hosting server when the event happened
        /// </summary>
        public long Tick { get; set; }
    }
}
