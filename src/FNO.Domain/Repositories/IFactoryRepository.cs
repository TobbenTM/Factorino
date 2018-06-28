using FNO.Domain.Models;
using System;

namespace FNO.Domain.Repositories
{
    public interface IFactoryRepository
    {
        Factory GetFactory(Guid factoryId);
    }
}
