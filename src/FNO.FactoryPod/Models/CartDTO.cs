using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    internal class CartDTO
    {
        [JsonProperty("cart_type")]
        public string CartType { get; set; }

        [JsonProperty("inventory")]
        public LuaItemStackDTO[] Inventory { get; set; }
    }
}
