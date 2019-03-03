using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public PlayerRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Player> GetPlayer(string steamId, Expression<Func<Player, object>> include = null)
        {
            return _dbContext.Players
                .Include(include)
                .FirstOrDefaultAsync(p => p.SteamId.Equals(steamId));
        }

        public Task<Player> GetPlayer(Guid playerId, Expression<Func<Player, object>> include = null)
        {
            return _dbContext.Players
                .Include(include)
                .FirstOrDefaultAsync(p => p.PlayerId.Equals(playerId));
        }

        public Task<Player> GetPlayer(ClaimsPrincipal user, Expression<Func<Player, object>> include = null)
        {
            var id = Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return GetPlayer(id, include);
        }

        public async Task<IEnumerable<CorporationInvitation>> GetInvitations(ClaimsPrincipal user)
        {
            var player = await GetPlayer(user);
            return player.Invitations;
        }
    }
}
