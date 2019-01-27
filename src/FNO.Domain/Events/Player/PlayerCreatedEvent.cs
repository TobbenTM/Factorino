namespace FNO.Domain.Events.Player
{
    public class PlayerCreatedEvent : EntityEvent
    {
        public string Name { get; set; }
        public string SteamId { get; set; }

        public string ProfileURL { get; set; }
        public string Avatar { get; set; }
        public string AvatarMedium { get; set; }
        public string AvatarFull { get; set; }

        public PlayerCreatedEvent()
        {
        }

        public PlayerCreatedEvent(Models.Player player) : base(player.PlayerId, player)
        {
            Name = player.Name;
            SteamId = player.SteamId;
            ProfileURL = player.ProfileURL;
            Avatar = player.Avatar;
            AvatarMedium = player.AvatarMedium;
            AvatarFull = player.AvatarFull;
        }
    }
}
