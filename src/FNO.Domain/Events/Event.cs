using FNO.Domain.Models;

namespace FNO.Domain.Events
{
    public abstract class Event : IEvent
    {
        public EventInitiator Initiator { get; set; }
        public EventMetadata Metadata { get; set; }

        public void Enrich(EventMetadata metadata)
        {
            Metadata = metadata;
        }

        public EventMetadata GetMetadata() => Metadata;
    }
}
