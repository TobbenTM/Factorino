using FNO.Domain.Models;
using System;
using System.Threading.Tasks;

namespace FNO.Domain.Repositories
{
    public interface ICorporationRepository
    {
        Task<Corporation> GetCorporation(Guid corporationId);
    }
}
