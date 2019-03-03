using FNO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(string steamId, Expression<Func<Player, object>> include = null);
        Task<Player> GetPlayer(Guid playerId, Expression<Func<Player, object>> include = null);
        Task<Player> GetPlayer(ClaimsPrincipal user, Expression<Func<Player, object>> include = null);
        Task<IEnumerable<CorporationInvitation>> GetInvitations(ClaimsPrincipal user);
    }
}
