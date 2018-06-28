using System;

namespace FNO.Domain.Models
{
    public class CorporationResearch
    {
        public int Id { get; set; }
        public long ResearchedAt { get; set; }

        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }

        public string ResearchId { get; set; }
        public FactorioTechnology Research { get; set; }
    }
}
