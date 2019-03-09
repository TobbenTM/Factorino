using FNO.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IFactoryLocationRepository
    {
        Task<IEnumerable<FactoryLocation>> GetAll();
    }
}
