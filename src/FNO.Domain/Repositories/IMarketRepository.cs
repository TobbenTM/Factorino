using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public interface IMarketRepository
    {
        Task<MarketOrder> GetOrder(Guid orderId);
        Task<IEnumerable<MarketOrder>> GetOrders(Player player);
    }
}