using System;

namespace FNO.Domain.Models
{
    public class WarehouseInventory
    {
        public Guid WarehouseInventoryId { get; set; }
        
        public int Quantity { get; set; }

        public string ItemId { get; set; }
        public FactorioEntity Item { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        //public Guid WarehouseId { get; set; }
        //public Warehouse Warehouse { get; set; }

        // public Guid CorporationId { get; set; }
        // public Corporation Corporation { get; set; }
    }
}
