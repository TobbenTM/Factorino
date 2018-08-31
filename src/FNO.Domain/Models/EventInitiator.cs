using System;

namespace FNO.Domain.Models
{
    public class EventInitiator
    {
        public Guid PlayerId { get; set; } = Guid.Empty;
        public string PlayerName { get; set; } = "<system>";

        public EventInitiator()
        {
        }

        public EventInitiator(Player player)
        {
            PlayerId = player.PlayerId;
            PlayerName = player.Name;
        }
    }
}
