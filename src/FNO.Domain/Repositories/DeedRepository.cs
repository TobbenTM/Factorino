using System;
using System.Threading.Tasks;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FNO.Domain.Repositories
{
    public class DeedRepository : IDeedRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public DeedRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Deed> Get(Guid deedId)
        {
            return _dbContext.Deeds.FirstOrDefaultAsync(d => d.DeedId == deedId);
        }
    }
}
