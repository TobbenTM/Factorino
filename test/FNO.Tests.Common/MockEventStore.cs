using FNO.Domain.Events;
using FNO.Domain.Models;
using FNO.EventSourcing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Tests.Common
{
    internal class MockEventStore : IEventStore
    {
        public List<IEvent> EventsProduced { get; } = new List<IEvent>();

        public Task<EventMetadata[]> ProduceAsync(params IEvent[] events)
        {
            EventsProduced.AddRange(events);
            return Task.FromResult(new EventMetadata[events.Length]);
        }

        public Task<EventMetadata[]> ProduceAsync(string topic, params IEvent[] events)
        {
            EventsProduced.AddRange(events);
            return Task.FromResult(new EventMetadata[events.Length]);
        }
    }
}
