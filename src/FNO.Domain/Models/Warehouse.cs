using System;
using System.Collections.Generic;

namespace FNO.Domain.Models
{
    public class Warehouse
    {
        public Guid WarehouseId { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        public IList<WarehouseInventory> Inventory { get; set; }
    }
}
