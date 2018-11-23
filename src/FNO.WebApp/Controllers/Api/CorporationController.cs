using FNO.Domain.Models;
using FNO.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/corporation")]
    public class CorporationController : Controller
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly ICorporationRepository _corporationRepo;

        public CorporationController(
            ICorporationRepository corporationRepo,
            IPlayerRepository playerRepository)
        {
            _corporationRepo = corporationRepo;
            _playerRepo = playerRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<Corporation> Get()
        {
            var player = await _playerRepo.GetPlayer(User);
            return player.Corporation;
        }
    }
}
