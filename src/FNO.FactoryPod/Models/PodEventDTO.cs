using Newtonsoft.Json;

namespace FNO.FactoryPod.Models
{
    internal class PodEventDTO
    {
        /// <summary>
        /// Present on all events
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Present on all events
        /// </summary>
        [JsonProperty("tick")]
        public long Tick { get; set; }

        /// <summary>
        /// Not present on the following events:
        /// * on_research_started
        /// * on_research_finished
        /// </summary>
        [JsonProperty("player_name")]
        public string PlayerName { get; set; }

        /// <summary>
        /// Present on the following events:
        /// * on_console_chat
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("entity")]
        public LuaEntityDTO Entity { get; set; }

        /// <summary>
        /// Present on the following events:
        /// * on_research_started
        /// * on_research_finished
        /// </summary>
        [JsonProperty("technology")]
        public LuaTechnologyDTO Technology { get; set; }

        /// <summary>
        /// Present on the following events:
        /// * on_player_mined_item
        /// </summary>
        [JsonProperty("item_stack")]
        public LuaItemStackDTO ItemStack { get; set; }

        /// <summary>
        /// Present on the following events:
        /// * factorino_outgoing_train
        /// </summary>
        [JsonProperty("train_name")]
        public string TrainName { get; set; }

        /// <summary>
        /// Present on the following events:
        /// * factorino_outgoing_train
        /// </summary>
        [JsonProperty("inventory")]
        public LuaItemStackDTO[] Inventory { get; set; }

        public override string ToString()
        {
            return $"PodEvent: {Type}, tick: {Tick}, player: {PlayerName ?? "<system>"}";
        }
    }
}
