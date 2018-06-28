using System;
using System.Linq;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public class CorporationRepository : ICorporationRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public CorporationRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Corporation GetCorporation(Guid corporationId)
        {
            return _dbContext.Corporations.FirstOrDefault(c => c.CorporationId.Equals(corporationId));
        }
    }
}
