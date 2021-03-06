﻿using System;

namespace FNO.Domain.Events.Factory
{
    public abstract class FactoryActivityBaseEvent : EntityEvent
    {
        protected FactoryActivityBaseEvent()
        {
        }

        protected FactoryActivityBaseEvent(Guid factoryId, string type, long tick) : base(factoryId, null)
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

        public override string ReadableEvent => $"{Initiator?.PlayerName ?? "An unknown entity"} caused a {Type} event for factory {EntityId}";
    }
}
