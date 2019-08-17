using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Models;

namespace FNO.Domain.Repositories
{
    public interface IShippingRepository
    {
        Task<IEnumerable<Shipment>> GetShipments(Player player);
    }
}
