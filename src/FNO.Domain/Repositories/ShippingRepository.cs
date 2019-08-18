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

        public async Task<IEnumerable<Shipment>> GetShipments(Player player)
        {
            return await _dbContext.Shipments
                .Where(s => s.OwnerId == player.PlayerId)
                .ToListAsync();
        }
    }
}
