using System;
using System.Collections.Generic;

namespace FNO.Domain.Models
{
    public class Warehouse
    {
        public Guid WarehouseId { get; set; }

        public Guid CorporationId { get; set; }
        public Corporation Corporation { get; set; }

        public IList<WarehouseInventory> Inventory { get; set; }

        public Warehouse()
        {

        }
    }
}
