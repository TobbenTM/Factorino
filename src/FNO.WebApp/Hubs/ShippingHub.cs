using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authorization;

namespace FNO.WebApp.Hubs
{
    public class ShippingHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IShippingRepository _repo;
        private readonly IEntityRepository _entityRepo;
        private readonly IEventStore _eventStore;

        public ShippingHub(
            IPlayerRepository playerRepository,
            IShippingRepository repo,
            IEntityRepository entityRepo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _repo = repo;
            _entityRepo = entityRepo;
            _eventStore = eventStore;
        }

        [Authorize]
        public async Task<IEnumerable<Shipment>> GetShipments()
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var shipments = await _repo.GetShipments(player);
            foreach (var shipment in shipments)
            {
                _entityRepo.Enrich(shipment.Carts.SelectMany(c => c.Inventory));
            }
            await Subscribe(shipments.Select(s => s.ShipmentId));
            return shipments;
        }
    }
}
