using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Factory> GetFactory(Guid factoryId)
        {
            return _dbContext.Factories
                .FirstOrDefaultAsync(f => f.FactoryId.Equals(factoryId));
        }

        // TODO: This should not await the task, just return it
        public async Task<IEnumerable<Factory>> GetFactories(Player player)
        {
            return await _dbContext.Factories
                .Include(f => f.Location)
                .Include(f => f.Owner)
                .Include(f => f.CurrentlyResearching)
                .Where(f => f.OwnerId == player.PlayerId)
                .Where(f => f.State != FactoryState.Destroyed)
                .ToListAsync();
        }
    }
}
