using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(string steamId);
        Task<Player> GetPlayer(Guid playerId);
        Task<Player> GetPlayer(ClaimsPrincipal user);
        Task<List<Player>> GetWealthiestPlayers();
        Task<IEnumerable<CorporationInvitation>> GetInvitations(ClaimsPrincipal user);
        Task<IEnumerable<WarehouseInventory>> GetInventory(Player player);
    }
}
