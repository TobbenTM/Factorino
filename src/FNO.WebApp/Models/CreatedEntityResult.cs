using System;

namespace FNO.WebApp.Models
{
    public class CreatedEntityResult : EventResult
    {
        public Guid EntityId { get; set; }
    }
}
