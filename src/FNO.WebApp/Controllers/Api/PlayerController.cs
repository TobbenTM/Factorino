using FNO.Domain.Events;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.WebApp.Filters;
using FNO.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _repo;
        private readonly IEventStore _eventStore;

        public PlayerController(IPlayerRepository repo, IEventStore eventStore)
        {
            _repo = repo;
            _eventStore = eventStore;
        }

        [HttpGet]
        [Authorize]
        public async Task<Player> Get()
        {
            var player = await _repo.GetPlayer(User);
            return player;
        }

        [HttpGet("inventory")]
        [Authorize]
        public async Task<IEnumerable<WarehouseInventory>> GetInventory()
        {
            var player = await _repo.GetPlayer(User);
            return await _repo.GetInventory(player);
        }

        [HttpGet("invitations")]
        public async Task<IEnumerable<CorporationInvitation>> GetInvitations()
        {
            var invitations = await _repo.GetInvitations(User);
            return invitations;
        }

        [HttpDelete("corporation")]
        [Authorize]
        [ValidateAntiForgeryToken]
        [EnsureConsumerConsistency]
        public async Task<EventResult> LeaveCorporation(Guid corporationId)
        {
            var player = await _repo.GetPlayer(User);

            var evnts = new IEvent[]
            {
                new PlayerLeftCorporationEvent(player.PlayerId, corporationId, player),
            };
            var results = await _eventStore.ProduceAsync(evnts);

            return new EventResult
            {
                Events = evnts,
                Results = results,
            };
        }
    }
}
