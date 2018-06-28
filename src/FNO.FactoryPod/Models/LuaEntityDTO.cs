using FNO.Domain.Models;
using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    class LuaEntityDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("backer_name")]
        public string BackerName { get; set; }

        public static implicit operator LuaEntity(LuaEntityDTO dto)
        {
            return new LuaEntity
            {
                Name = dto.Name,
                BackerName = dto.BackerName,
            };
        }
    }
}
