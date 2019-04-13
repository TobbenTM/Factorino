using System.Collections.Generic;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    // TODO: Rename to item repository or something?
    public interface IEntityRepository
    {
        IEnumerable<FactorioEntity> Search(string query);
    }
}