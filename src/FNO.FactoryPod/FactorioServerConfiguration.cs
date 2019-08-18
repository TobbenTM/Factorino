using Newtonsoft.Json;
using System;

namespace FNO.FactoryPod
{
    /// <summary>
    /// Server settings template with somewhat reasonable defaults
    /// </summary>
    internal class FactorioServerConfiguration
    {
        /// <summary>
        /// Name of the game as it will appear in the game listing
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = "Factorino";

        /// <summary>
        /// Description of the game that will appear in the listing
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Maximum number of players allowed, admins can join even a full server. 0 means unlimited
        /// </summary>
        [JsonProperty("max_players")]
        public int MaxPlayers { get; set; } = 0;

        [JsonProperty("visibility")]
        public FactorioServerVisibilityConfiguration Visibility { get; set; } = new FactorioServerVisibilityConfiguration();

        // Skipping credentials as we don't need them (username, password, token)

        [JsonProperty("game_password")]
        public string GamePassword { get; set; } = "";

        /// <summary>
        /// When set to true, the server will only allow clients that have a valid Factorio.com account
        /// </summary>
        [JsonProperty("require_user_verification")]
        public bool RequireUserVerification { get; set; } = true;

        /// <summary>
        /// Optional, default value is 0. 0 means unlimited
        /// </summary>
        [JsonProperty("max_upload_in_kilobytes_per_second")]
        public int MaxUploadInKilobytesPerSecond { get; set; } = 0;

        /// <summary>
        /// Optional, one tick is 16ms in default speed, default value is 0. 0 means no minimum
        /// </summary>
        [JsonProperty("minimum_latency_in_ticks")]
        public int MinimumLatencyInTicks { get; set; } = 0;

        /// <summary>
        /// Players that played on this map already can join even when the max player limit was reached
        /// </summary>
        [JsonProperty("ignore_player_limit_for_returning_players")]
        public bool IgnorePlayerLimitForReturningPlayers { get; set; } = false;

        /// <summary>
        /// Possible values are, true, false and admins-only
        /// </summary>
        [JsonProperty("allow_commands")]
        public string AllowCommands { get; set; } = "admins-only";

        /// <summary>
        /// Autosave interval in minutes
        /// </summary>
        [JsonProperty("autosave_interval")]
        public int AutosaveInterval { get; set; } = 10;

        /// <summary>
        /// Server autosave slots, it is cycled through when the server autosaves
        /// </summary>
        [JsonProperty("autosave_slots")]
        public int AutosaveSlots { get; set; } = 5;

        /// <summary>
        /// How many minutes until someone is kicked when doing nothing, 0 for never
        /// </summary>
        [JsonProperty("afk_autokick_interval")]
        public int AFKAutokickInterval { get; set; } = 0;

        /// <summary>
        /// Whether should the server be paused when no players are present
        /// </summary>
        [JsonProperty("auto_pause")]
        public bool AutoPause { get; set; } = false;

        [JsonProperty("only_admins_can_pause_the_game")]
        public bool OnlyAdminsCanPauseTheGame { get; set; } = true;

        /// <summary>
        /// Whether autosaves should be saved only on server or also on all connected clients. Default is true
        /// </summary>
        [JsonProperty("autosave_only_on_server")]
        public bool AutosaveOnlyOnServer { get; set; } = true;

        /// <summary>
        /// Highly experimental feature, enable only at your own risk of losing your saves
        /// On UNIX systems, server will fork itself to create an autosave
        /// Autosaving on connected Windows clients will be disabled regardless of autosave_only_on_server option
        /// </summary>
        [JsonProperty("non_blocking_saving")]
        public bool NonBlockingSave { get; set; } = false;

        /// <summary>
        /// List of case insensitive usernames, that will be promoted immediately
        /// </summary>
        [JsonProperty("admins")]
        public string[] Admins { get; set; } = Array.Empty<string>();
    }

    internal class FactorioServerVisibilityConfiguration
    {
        /// <summary>
        /// Game will be published on the official Factorio matching server
        /// </summary>
        [JsonProperty("public")]
        public bool Public { get; set; } = false;

        /// <summary>
        /// Game will be broadcast on LAN
        /// </summary>
        [JsonProperty("lan")]
        public bool LAN { get; set; } = false;
    }
}
