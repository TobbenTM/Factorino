using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers.Api
{
    [ApiController]
    [Route("api/factory")]
    public class FactoryController : Controller
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IFactoryRepository _factoryRepo;
        private readonly IFactoryLocationRepository _factoryLocationRepo;
        private readonly IEventStore _eventStore;

        public FactoryController(
            IPlayerRepository playerRepo,
            IFactoryRepository factoryRepo,
            IFactoryLocationRepository factoryLocationRepo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepo;
            _factoryRepo = factoryRepo;
            _factoryLocationRepo = factoryLocationRepo;
            _eventStore = eventStore;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Factory>> Get()
        {
            var player = await _playerRepo.GetPlayer(User);
            var factories = await _factoryRepo.GetFactories(player);
            return factories;
        }

        [HttpGet("locations")]
        public async Task<IEnumerable<FactoryLocation>> GetLocations()
        {
            return await _factoryLocationRepo.GetAll();
        }
    }
}
