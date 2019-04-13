using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FNO.Domain.Models.Shipping;
using Newtonsoft.Json;

namespace FNO.Domain.Models
{
    public class Shipment
    {
        public Guid ShipmentId { get; set; }
        public ShipmentState State { get; set; }

        public Guid FactoryId { get; set; }
        public Factory Factory { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        // TODO: Refactor this hack please
        [JsonIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CartsData { get; set; }
        [NotMapped]
        public IEnumerable<Cart> Carts
        {
            get => CartsData == null ? null : JsonConvert.DeserializeObject<IEnumerable<Cart>>(CartsData);
            set => CartsData = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public string DestinationStation { get; set; }

        // TODO: Refactor this hack please
        [JsonIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string WaitConditionsData { get; set; }
        [NotMapped]
        public IEnumerable<WaitCondition> WaitConditions
        {
            get => WaitConditionsData == null ? null : JsonConvert.DeserializeObject<IEnumerable<WaitCondition>>(WaitConditionsData);
            set => WaitConditionsData = value == null ? null : JsonConvert.SerializeObject(value);
        }
    }
}
