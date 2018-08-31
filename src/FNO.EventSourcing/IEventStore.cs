using FNO.Domain.Events;
using FNO.Domain.Models;
using System.Threading.Tasks;

namespace FNO.EventSourcing
{
    public interface IEventStore
    {
        Task<EventMetadata[]> ProduceAsync(params IEvent[] events);
        Task<EventMetadata[]> ProduceAsync(string topic, params IEvent[] events);
    }
}
