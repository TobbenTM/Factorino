using FNO.Domain.Events.Factory;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace FNO.WebApp.Hubs
{
    [Authorize]
    public class FactoryCreateHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IFactoryLocationRepository _locationRepo;
        private readonly IEventStore _eventStore;

        public FactoryCreateHub(
            IPlayerRepository playerRepository,
            IFactoryLocationRepository locationRepo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _locationRepo = locationRepo;
            _eventStore = eventStore;
        }

        public async Task<CreatedEntityResult> CreateFactory(Guid locationId)
        {
            var player = await _playerRepo.GetPlayer(Context.User);

            var factoryId = Guid.NewGuid();
            var location = await _locationRepo.Get(locationId);

            // We need to receive all events for this factory from now on
            await Subscribe(factoryId);
            
            var evnt = new FactoryCreatedEvent(factoryId, locationId, location.Seed, player);
            var results = await _eventStore.ProduceAsync(evnt);

            return new CreatedEntityResult
            {
                Events = new[] { evnt },
                Results = results,
                EntityId = factoryId,
            };
        }
    }
}
