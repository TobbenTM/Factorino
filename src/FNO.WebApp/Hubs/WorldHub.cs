using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;
using FNO.Domain.Repositories;

namespace FNO.WebApp.Hubs
{
    public class WorldHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;

        public WorldHub(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        public Task<List<Player>> GetHighscores()
        {
            return _playerRepo.GetWealthiestPlayers();
        }
    }
}
