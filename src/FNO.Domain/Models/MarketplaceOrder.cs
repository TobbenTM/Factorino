using System;
using System.ComponentModel.DataAnnotations;

namespace FNO.Domain.Models
{
    public class MarketOrder
    {
        [Key]
        public Guid OrderId { get; set; }

        public OrderType OrderType { get; set; }

        public Guid CorporationId { get; set; }

        public string ItemId { get; set; }
        public FactorioEntity Item { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
