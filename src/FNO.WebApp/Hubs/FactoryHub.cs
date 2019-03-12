using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.WebApp.Hubs
{
    public class FactoryHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IFactoryRepository _repo;
        private readonly IEventStore _eventStore;

        public FactoryHub(
            IPlayerRepository playerRepository,
            IFactoryRepository repo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _repo = repo;
            _eventStore = eventStore;
        }

        [Authorize]
        public async Task<IEnumerable<Factory>> GetFactories()
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var factories = await _repo.GetFactories(player);
            foreach (var factory in factories)
            {
                await Subscribe(factory.FactoryId);
            }
            return factories;
        }

        [Authorize]
        public async Task<EventResult> DeleteFactory(Guid factoryId)
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var factory = await _repo.GetFactory(factoryId);
            if (factory.OwnerId != player.PlayerId)
            {
                throw new UnauthorizedAccessException("User does not own the factory");
            }
            if (factory.State == FactoryState.Destroying || factory.State == FactoryState.Destroyed)
            {
                // TODO: Do we need to check for this?
            }

            var evnt = new FactoryDestroyedEvent(factoryId, player);
            var results = await _eventStore.ProduceAsync(evnt);

            return new EventResult
            {
                Events = new[] { evnt },
                Results = results,
            };
        }
    }
}
