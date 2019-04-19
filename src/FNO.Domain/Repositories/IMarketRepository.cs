using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;

namespace FNO.Domain.Repositories
{
    public interface IMarketRepository
    {
        Task<MarketOrder> GetOrder(Guid orderId);
        Task<IEnumerable<MarketOrder>> GetOrders(Player player);
        Task<Page<MarketOrder>> SearchOrders(int pageIndex, int pageSize, OrderSearchFilter filter);
    }
}