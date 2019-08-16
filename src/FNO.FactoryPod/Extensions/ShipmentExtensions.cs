using System.Linq;
using FNO.Domain.Models;
using FNO.FactoryPod.Models;

namespace FNO.FactoryPod.Extensions
{
    internal static class ShipmentExtensions
    {
        public static ShipmentDTO ToDTO(this Shipment shipment)
        {
            return new ShipmentDTO
            {
                ShipmentId = shipment.ShipmentId,
                DestinationStation = shipment.DestinationStation,
                Carts = shipment.Carts.Select(c => new CartDTO
                {
                    CartType = c.CartType.ToString(),
                    Inventory = c.Inventory.Select(i => new LuaItemStackDTO
                    {
                        Name = i.Name,
                        Count = (int)i.Count,
                    }).ToArray(),
                }).ToArray(),
                WaitConditions = shipment.WaitConditions.Select(w => new WaitConditionDTO
                {
                    CompareType = w.CompareType.ToString(),
                    Ticks = w.Ticks,
                    Type = w.Type.ToString(),
                }).ToArray(),
            };
        }
    }
}
