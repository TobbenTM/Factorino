using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public Task<FactoryLocation> Get(Guid locationId)
        {
            return _dbContext.FactoryLocations
                .FirstOrDefaultAsync(l => l.LocationId == locationId);
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
