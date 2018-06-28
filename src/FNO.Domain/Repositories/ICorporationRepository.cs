using FNO.Domain.Models;
using System;

namespace FNO.Domain.Repositories
{
    public interface ICorporationRepository
    {
        Corporation GetCorporation(Guid corporationId);
    }
}
