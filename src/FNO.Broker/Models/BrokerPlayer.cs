using System.Collections.Generic;
using FNO.Domain.Models;

namespace FNO.Broker.Models
{
    /// <summary>
    /// Variant of the player optimized for easier warehouse management
    /// </summary>
    public class BrokerPlayer : Player
    {
        /// <summary>
        /// Warehouse contents, indexed by item id
        /// </summary>
        public Dictionary<string, WarehouseInventory> Inventory { get; set; }

        public BrokerPlayer()
        {
            Inventory = new Dictionary<string, WarehouseInventory>();
        }
    }
}
