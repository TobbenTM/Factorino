using FNO.Domain.Events;
using FNO.Domain.Models;
using System.Collections.Generic;

namespace FNO.WebApp.Models
{
    public class EventResult : IEventResult
    {
        public EventMetadata[] Results { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}
