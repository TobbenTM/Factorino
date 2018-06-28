using System;
using System.Collections.Generic;

namespace FNO.Domain.Models
{
    public class Corporation
    {
        public Guid CorporationId { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public Warehouse Warehouse { get; set; }
        public IList<Player> Members { get; set; }
    }
}
