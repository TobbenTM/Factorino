using FNO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(string steamId);
        Task<Player> GetPlayer(Guid playerId);
        Task<Player> GetPlayer(ClaimsPrincipal user);
        Task<IEnumerable<CorporationInvitation>> GetInvitations(ClaimsPrincipal user);
    }
}
