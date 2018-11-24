using FNO.Domain.Events;
using FNO.Domain.Models;
using System;
using System.Collections.Generic;

namespace FNO.WebApp.Models
{
    public class CreatedEntityResult
    {
        public Guid EntityId { get; set; }
        public EventMetadata[] Results { get; set; }
        public IEnumerable<IEvent> Events { get; set; }
    }
}
