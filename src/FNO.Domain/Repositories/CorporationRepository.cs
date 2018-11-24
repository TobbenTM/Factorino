using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public class CorporationRepository : ICorporationRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public CorporationRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Corporation> GetCorporation(Guid corporationId)
        {
            return _dbContext.Corporations
                .Include(c => c.Members)
                .Include(c => c.CreatedByPlayer)
                .FirstOrDefaultAsync(c => c.CorporationId.Equals(corporationId));
        }
    }
}
