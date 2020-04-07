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
        private readonly IDeedRepository _deedRepository;
        private readonly IEventStore _eventStore;

        public FactoryCreateHub(
            IPlayerRepository playerRepository,
            IDeedRepository deedRepository,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _deedRepository = deedRepository;
            _eventStore = eventStore;
        }

        public async Task<CreatedEntityResult> CreateFactory(Guid deedId)
        {
            var player = await _playerRepo.GetPlayer(Context.User);

            var factoryId = Guid.NewGuid();
            var deed = await _deedRepository.Get(deedId);

            if (deed.OwnerId != player.PlayerId)
            {
                throw new UnauthorizedAccessException($"Player {player.Name} ({player.PlayerId}) does not own the deed {deed.DeedId}!");
            }

            // We need to receive all events for this factory from now on
            await Subscribe(factoryId);
            
            var evnt = new FactoryCreatedEvent(factoryId, deedId, player);
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
