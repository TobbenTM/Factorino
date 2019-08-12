using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.EventSourcing;

namespace FNO.Broker.EventHandlers
{
    public class PlayerEventHandler : IHandler,
        IEventHandler<PlayerCreatedEvent>,
        IEventHandler<PlayerBalanceChangedEvent>,
        IEventHandler<PlayerInventoryChangedEvent>
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

        public Task Handle(PlayerBalanceChangedEvent evnt)
        {
            if (evnt.Metadata.SourceAssembly == Assembly.GetExecutingAssembly().FullName)
            {
                // We don't process these events if they come from this assembly
                return Task.CompletedTask;
            }

            var player = _state.Players[evnt.EntityId];
            player.Credits += evnt.BalanceChange;
            return Task.CompletedTask;
        }

        public Task Handle(PlayerInventoryChangedEvent evnt)
        {
            if (evnt.Metadata.SourceAssembly == Assembly.GetExecutingAssembly().FullName)
            {
                // We don't process these events if they come from this assembly
                return Task.CompletedTask;
            }

            var player = _state.Players[evnt.EntityId];

            foreach (var stack in evnt.InventoryChange)
            {
                if (player.Inventory.TryGetValue(stack.Name, out var inventory))
                {
                    inventory.Quantity += stack.Count;
                }
                else
                {
                    player.Inventory.Add(stack.Name, new WarehouseInventory
                    {
                        ItemId = stack.Name,
                        Quantity = stack.Count,
                    });
                }
            }

            return Task.CompletedTask;
        }
    }
}
