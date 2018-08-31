using FNO.Domain.Models;

namespace FNO.Domain.Events
{
    public interface IEvent
    {
        void Enrich(EventMetadata metadata);
    }
}
