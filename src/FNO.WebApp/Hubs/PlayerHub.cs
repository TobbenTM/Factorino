using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authorization;

namespace FNO.WebApp.Hubs
{
    public class PlayerHub : EventHandlerHub
    {
        private readonly IPlayerRepository _repo;
        private readonly IEventStore _eventStore;

        public PlayerHub(
            IPlayerRepository playerRepository,
            IEventStore eventStore)
        {
            _repo = playerRepository;
            _eventStore = eventStore;
        }

        [Authorize]
        public async Task<Player> GetPlayer()
        {
            var player = await _repo.GetPlayer(Context.User);
            await Subscribe(player.PlayerId);
            return player;
        }

        [Authorize]
        public async Task<IEnumerable<WarehouseInventory>> GetInventory()
        {
            var player = await _repo.GetPlayer(Context.User);
            await Subscribe(player.PlayerId);
            var inventory = await _repo.GetInventory(player);
            return inventory;
        }
    }
}
