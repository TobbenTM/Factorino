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
        Task<IEnumerable<WarehouseInventory>> GetInventory(Player player);
        Task<IEnumerable<Shipment>> GetShipments(Player player);
        Task<IEnumerable<MarketOrder>> GetOrders(Player player);
    }
}
