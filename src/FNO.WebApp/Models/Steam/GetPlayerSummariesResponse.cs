using Newtonsoft.Json;

namespace FNO.WebApp.Models.Steam
{
    public class GetPlayerSummariesResponse
    {
        public class SteamPlayer
        {
            [JsonProperty("steamid")]
            public string SteamId { get; set; }

            [JsonProperty("personname")]
            public string PersonName { get; set; }

            [JsonProperty("profileurl")]
            public string ProfileURL { get; set; }

            [JsonProperty("avatar")]
            public string Avatar { get; set; }

            [JsonProperty("avatarmedium")]
            public string AvatarMedium { get; set; }

            [JsonProperty("avatarfull")]
            public string AvatarFull { get; set; }

            [JsonProperty("realname")]
            public string RealName { get; set; }

            [JsonProperty("loccountrycode")]
            public string CountryCode { get; set; }
        }

        [JsonProperty("players")]
        public SteamPlayer[] Players { get; set; }
    }
}
