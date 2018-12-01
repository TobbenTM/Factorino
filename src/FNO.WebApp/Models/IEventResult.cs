using FNO.Domain.Events;
using FNO.Domain.Models;
using System.Collections.Generic;

namespace FNO.WebApp.Models
{
    public interface IEventResult
    {
        EventMetadata[] Results { get; set; }
        IEnumerable<IEvent> Events { get; set; }
    }
}
