using FNO.Domain.Models;
using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    class LuaTechnologyDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        public static implicit operator LuaTechnology(LuaTechnologyDTO dto)
        {
            return new LuaTechnology
            {
                Name = dto.Name,
                Level = dto.Level,
            };
        }
    }
}
