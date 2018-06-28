using System;
using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    internal class TransactionDTO
    {
        [JsonProperty("transaction_id")]
        public Guid TransactionId { get; set; }

        [JsonProperty("destination_station")]
        public string DestinationStation { get; set; }

        [JsonProperty("wait_conditions")]
        public WaitConditionDTO[] WaitConditions { get; set; }

        [JsonProperty("carts")]
        public CartDTO[] Carts { get; set; }
    }
}
