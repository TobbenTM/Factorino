using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FNO.Domain.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public ShippingRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Shipment>> GetShipments(Player player)
        {
            return _dbContext.Shipments
                .Include(s => s.Factory)
                .ThenInclude(f => f.Deed)
                .Where(s => s.OwnerId == player.PlayerId)
                .ToListAsync();
        }
    }
}
