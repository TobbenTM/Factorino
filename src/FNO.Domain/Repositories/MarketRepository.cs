using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using Microsoft.EntityFrameworkCore;

namespace FNO.Domain.Repositories
{
    public class MarketRepository : IMarketRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public MarketRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<MarketOrder> GetOrder(Guid orderId)
        {
            return _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<MarketOrder>> GetOrders(Player player)
        {
            return await _dbContext.Orders
                .Include(o => o.Item)
                .Where(o => o.OwnerId == player.PlayerId)
                .ToListAsync();
        }

        public async Task<Page<MarketOrder>> SearchOrders(int pageIndex, int pageSize, OrderSearchFilter filter)
        {
            var ordersQuery = _dbContext.Orders
                .Include(o => o.Item)
                .Include(o => o.Owner)
                .Where(o => o.State != OrderState.Cancelled)
                .Where(o => o.OrderType == filter.OrderType);

            if (!string.IsNullOrEmpty(filter.ItemId))
            {
                ordersQuery = ordersQuery.Where(o => o.ItemId == filter.ItemId);
            }

            if (filter.MinPrice != null)
            {
                ordersQuery = ordersQuery.Where(o => o.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice != null)
            {
                ordersQuery = ordersQuery.Where(o => o.Price <= filter.MaxPrice.Value);
            }

            var count = ordersQuery.Count();
            return new Page<MarketOrder>
            {
                PageCount = (int)Math.Ceiling(count / (decimal)pageSize),
                PageIndex = pageIndex,
                ResultCount = count,
                Results = await ordersQuery.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync(),
            };
        }
    }
}
