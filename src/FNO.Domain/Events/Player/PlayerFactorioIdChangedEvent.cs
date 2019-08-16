using System;

namespace FNO.Domain.Events.Player
{
    public class PlayerFactorioIdChangedEvent : EntityEvent
    {
        public string FactorioId { get; set; }

        public PlayerFactorioIdChangedEvent()
        {
        }

        public PlayerFactorioIdChangedEvent(Guid playerId, Models.Player initiator) : base(playerId, initiator)
        {
        }
    }
}
