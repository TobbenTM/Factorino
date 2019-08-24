using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Exceptions;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authorization;

namespace FNO.WebApp.Hubs
{
    public class ShippingHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IShippingRepository _repo;
        private readonly IEntityRepository _entityRepo;
        private readonly IEventStore _eventStore;

        public ShippingHub(
            IPlayerRepository playerRepository,
            IFactoryRepository factoryRepository,
            IShippingRepository repo,
            IEntityRepository entityRepo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _factoryRepository = factoryRepository;
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

        [Authorize]
        public async Task<Shipment> CreateShipment(Shipment shipment)
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var factory = await _factoryRepository.GetFactory(shipment.FactoryId);

            if (factory == null)
            {
                throw new EntityNotFoundException(shipment.FactoryId);
            }

            var shipmentId = Guid.NewGuid();

            shipment.ShipmentId = shipmentId;
            shipment.WaitConditions = new[]
            {
                new WaitCondition
                {
                    CompareType = WaitConditionCompareType.Or,
                    Type = WaitConditionType.Empty,
                },
                new WaitCondition
                {
                    CompareType = WaitConditionCompareType.Or,
                    Type = WaitConditionType.Inactivity,
                    Ticks = 2400,
                },
            };

            // We need to receive all events for this shipment from now on
            await Subscribe(shipmentId);

            var evnt = new ShipmentRequestedEvent(shipmentId, shipment.FactoryId, player)
            {
                DestinationStation = shipment.DestinationStation,
                Carts = shipment.Carts.ToArray(),
                WaitConditions = shipment.WaitConditions.ToArray(),
            };
            await _eventStore.ProduceAsync(evnt);
            return shipment;
        }
    }
}
