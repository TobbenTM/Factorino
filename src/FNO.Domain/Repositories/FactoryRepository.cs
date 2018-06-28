using System;
using System.Linq;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Factory GetFactory(Guid factoryId)
        {
            return _dbContext.Factories.FirstOrDefault(f => f.FactoryId.Equals(factoryId));
        }
    }
}
