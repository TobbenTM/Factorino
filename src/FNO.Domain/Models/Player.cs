using System;

namespace FNO.Domain.Models
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }
    }
}
