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
        private readonly IEventStore _eventStore;

        public FactoryCreateHub(
            IPlayerRepository playerRepository,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _eventStore = eventStore;
        }

        public async Task<CreatedEntityResult> CreateFactory(string seed)
        {
            var player = await _playerRepo.GetPlayer(Context.User);

            var factoryId = Guid.NewGuid();

            var evnt = new FactoryCreatedEvent(factoryId, player);
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
