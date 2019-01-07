using FNO.Domain.Events;
using FNO.Domain.Events.Corporation;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.WebApp.Filters;
using FNO.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/corporation")]
    public class CorporationController : Controller
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly ICorporationRepository _corporationRepo;
        private readonly IEventStore _eventStore;

        public CorporationController(
            ICorporationRepository corporationRepo,
            IPlayerRepository playerRepository,
            IEventStore eventStore)
        {
            _corporationRepo = corporationRepo;
            _playerRepo = playerRepository;
            _eventStore = eventStore;
        }

        [HttpGet]
        [Authorize]
        public async Task<Corporation> Get()
        {
            var player = await _playerRepo.GetPlayer(User);
            if (player?.CorporationId == null) return null;
            var corporation = await _corporationRepo.GetCorporation(player.CorporationId.Value);
            return corporation;
        }

        [HttpPut]
        [Authorize]
        [ValidateAntiForgeryToken]
        [EnsureConsumerConsistency]
        public async Task<CreatedEntityResult> Create(Corporation corporation)
        {
            var player = await _playerRepo.GetPlayer(User);

            corporation.CorporationId = Guid.NewGuid();
            corporation.CreatedByPlayerId = player.PlayerId;

            var evnts = new IEvent[]
            {
                new CorporationCreatedEvent(corporation, player),
                new PlayerJoinedCorporationEvent(player.PlayerId, corporation.CorporationId, player),
            };
            var results = await _eventStore.ProduceAsync(evnts);

            return new CreatedEntityResult
            {
                Events = evnts,
                Results = results,
                EntityId = corporation.CorporationId,
            };
        }
    }
}
