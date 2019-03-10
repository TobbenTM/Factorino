using FNO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IFactoryLocationRepository
    {
        Task<FactoryLocation> Get(Guid locationId);
        Task<IEnumerable<FactoryLocation>> GetAll();
    }
}
