using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNO.Domain.Models
{
    public class FactoryLocation
    {
        [Key]
        public Guid LocationId { get; set; }
        public string Seed { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public IEnumerable<FactoryLocationResource> Resources { get; set; }

        [InverseProperty(nameof(Factory.Location))]
        public IEnumerable<Factory> Factories { get; set; }
    }
}
