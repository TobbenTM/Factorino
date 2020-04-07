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
                .Include(f => f.Deed)
                .FirstOrDefaultAsync(f => f.FactoryId.Equals(factoryId));
        }

        public Task<List<Factory>> GetFactories(Player player)
        {
            return  _dbContext.Factories
                .Include(f => f.Deed)
                .Include(f => f.Owner)
                .Include(f => f.CurrentlyResearching)
                .Where(f => f.OwnerId == player.PlayerId)
                .Where(f => f.State != FactoryState.Destroyed)
                .ToListAsync();
        }
    }
}
