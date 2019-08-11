using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNO.Domain.Models
{
    public class Corporation
    {
        public Guid CorporationId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public long Credits { get; set; }

        public Guid CreatedByPlayerId { get; set; }
        public Player CreatedByPlayer { get; set; }

        //public Warehouse Warehouse { get; set; }

        [InverseProperty("Corporation")]
        public IEnumerable<Player> Members { get; set; }

        public IEnumerable<WarehouseInventory> WarehouseInventory { get; set; }
    }
}
