using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public interface IShippingRepository
    {
        Task<List<Shipment>> GetShipments(Player player);
    }
}
