﻿using System;
using System.ComponentModel.DataAnnotations;
using FNO.Domain.Models.Market;

namespace FNO.Domain.Models
{
    public class MarketOrder
    {
        [Key]
        public Guid OrderId { get; set; }

        public OrderType OrderType { get; set; }
        public OrderState State { get; set; }
        public OrderCancellationReason CancellationReason { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        public string ItemId { get; set; }
        public FactorioEntity Item { get; set; }

        public long Quantity { get; set; }
        public long Price { get; set; }

        public long QuantityFulfilled { get; set; }
    }
}
