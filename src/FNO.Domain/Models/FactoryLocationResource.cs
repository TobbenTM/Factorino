using System;

namespace FNO.Domain.Models
{
    public class FactoryLocationResource
    {
        public string EntityId { get; set; }
        public FactorioEntity Entity { get; set; }

        public Guid LocationId { get; set; }
        public FactoryLocation Location { get; set; }
    }
}
