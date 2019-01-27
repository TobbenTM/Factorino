using Newtonsoft.Json;

namespace FNO.WebApp.Models.Steam
{
    public class SteamApiResponseWrapper<TResponse>
    {
        [JsonProperty("response")]
        public TResponse Response { get; set; }
    }
}
