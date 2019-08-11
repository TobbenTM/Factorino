using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.EventSourcing;

namespace FNO.Broker.EventHandlers
{
    public class PlayerEventHandler : IHandler,
        IEventHandler<PlayerCreatedEvent>
    {
        private readonly State _state;

        public PlayerEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(PlayerCreatedEvent evnt)
        {
            _state.Players.Add(evnt.EntityId, new BrokerPlayer
            {
                PlayerId = evnt.EntityId,
                Inventory = new Dictionary<string, WarehouseInventory>(),
            });
            return Task.CompletedTask;
        }
    }
}
