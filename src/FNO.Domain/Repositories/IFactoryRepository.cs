using FNO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface IFactoryRepository
    {
        Task<Factory> GetFactory(Guid factoryId);
        Task<List<Factory>> GetFactories(Player player);
    }
}
