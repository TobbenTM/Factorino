using System.Diagnostics;
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
        }

        public void Enrich(EventMetadata metadata)
        {
            Metadata = metadata;
        }

        public EventMetadata GetMetadata() => Metadata;
    }
}
