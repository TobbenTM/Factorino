using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    internal class WaitConditionDTO
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("compare_type")]
        public string CompareType { get; set; }

        /// <summary>
        /// Used when using the "time" type
        /// </summary>
        [JsonProperty("ticks")]
        public int Ticks { get; set; }
    }
}
