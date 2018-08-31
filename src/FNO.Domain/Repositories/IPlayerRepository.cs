using FNO.Domain.Models;
using System;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayer(string steamId);
        Task<Player> GetPlayer(Guid playerId);
    }
}
