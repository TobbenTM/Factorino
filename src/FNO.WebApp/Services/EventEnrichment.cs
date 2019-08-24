using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events.Player;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.Extensions.Configuration;

namespace FNO.WebApp.Services
{
    public class EventEnrichment : IEventHandler<PlayerInventoryChangedEvent>
    {
        private readonly IConfiguration _configuration;

        public EventEnrichment(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Handle(PlayerInventoryChangedEvent evnt)
        {
            // TODO: Make fully async and using proper DI
            using (var dbContext = ReadModelDbContext.CreateContext(_configuration))
            {
                var entityRepository = new EntityRepository(dbContext);
                entityRepository.Enrich(evnt.InventoryChange);
            }
            return Task.CompletedTask;
        }
    }
}
