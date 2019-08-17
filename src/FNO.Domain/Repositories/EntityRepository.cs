﻿using System.Collections.Generic;
using System.Linq;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public EntityRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<FactorioEntity> Search(string query)
        {
            return _dbContext.EntityLibrary
                .Where(e => e.Name.Contains(query))
                .ToList();
        }

        public FactorioEntity Get(string itemId)
        {
            return _dbContext.EntityLibrary
                .First(e => e.Name == itemId);
        }
    }
}
