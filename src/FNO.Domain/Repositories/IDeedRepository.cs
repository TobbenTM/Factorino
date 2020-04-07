using System;
using System.Threading.Tasks;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public interface IDeedRepository
    {
        Task<Deed> Get(Guid deedId);
    }
}
