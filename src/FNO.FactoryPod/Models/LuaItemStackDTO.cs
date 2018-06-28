using FNO.Domain.Models;
using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    internal class LuaItemStackDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        public static implicit operator LuaItemStack(LuaItemStackDTO dto)
        {
            return new LuaItemStack
            {
                Name = dto.Name,
                Count = dto.Count,
            };
        }
    }
}
