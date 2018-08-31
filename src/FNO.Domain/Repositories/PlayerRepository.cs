using System;
using System.Threading.Tasks;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FNO.Domain.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public PlayerRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Player> GetPlayer(string steamId)
        {
            return _dbContext.Players.FirstOrDefaultAsync(p => p.SteamId.Equals(steamId));
        }

        public Task<Player> GetPlayer(Guid playerId)
        {
            return _dbContext.Players.FirstOrDefaultAsync(p => p.PlayerId.Equals(playerId));
        }
    }
}
