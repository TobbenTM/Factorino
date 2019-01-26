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

        public EventInitiator(Player initiator)
        {
            if (initiator != null)
            {
                PlayerId = initiator.PlayerId;
                PlayerName = initiator.Name;
            }
        }

        public Player ToPlayer()
        {
            return new Player
            {
                PlayerId = PlayerId,
                Name = PlayerName,
            };
        }
    }
}
