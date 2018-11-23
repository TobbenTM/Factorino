using FNO.Domain.Models;
using FNO.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _repo;

        public PlayerController(IPlayerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize]
        public async Task<Player> Get()
        {
            var player = await _repo.GetPlayer(User);
            return player;
        }

        [HttpGet, Route("invitations")]
        public async Task<IEnumerable<CorporationInvitation>> GetInvitations()
        {
            var invitations = await _repo.GetInvitations(User);
            return invitations;
        }
    }
}
