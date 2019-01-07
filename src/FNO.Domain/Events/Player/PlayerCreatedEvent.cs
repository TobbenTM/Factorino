namespace FNO.Domain.Events.Player
{
    public class PlayerCreatedEvent : EntityEvent
    {
        public string Name { get; set; }
        public string SteamId { get; set; }

        public PlayerCreatedEvent()
        {
        }

        public PlayerCreatedEvent(Models.Player player) : base(player.PlayerId, player)
        {
            Name = player.Name;
            SteamId = player.SteamId;
        }
    }
}
