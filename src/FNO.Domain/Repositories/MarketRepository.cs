using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Models;
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
    }
}
