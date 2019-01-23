using System;
using FNO.Domain.Models;

namespace FNO.Domain.Events
{
    public abstract class Event : IEvent
    {
        public EventInitiator Initiator { get; set; }
        public EventMetadata Metadata { get; set; }

        protected Event()
        {
        }

        protected Event(Models.Player initiator)
        {
            Initiator = new EventInitiator(initiator);
            Metadata = new EventMetadata
            {
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
        }

        public void Enrich(EventMetadata metadata)
        {
            if (Metadata != null)
            {
                Metadata.Enrich(metadata);
            }
            else
            {
                Metadata = metadata;
            }
        }

        public EventMetadata GetMetadata() => Metadata;
    }
}
