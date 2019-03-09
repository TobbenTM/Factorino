using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public class FactoryLocationRepository : IFactoryLocationRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryLocationRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FactoryLocation>> GetAll()
        {
            return await _dbContext.FactoryLocations
                .Include(l => l.Resources)
                .ThenInclude(r => r.Entity)
                .ToListAsync();
        }
    }
}
