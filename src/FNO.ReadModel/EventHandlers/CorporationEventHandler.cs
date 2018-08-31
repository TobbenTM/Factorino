using FNO.Domain;
using FNO.Domain.Events.Corporation;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Serilog;
using System.Threading.Tasks;

namespace FNO.ReadModel.EventHandlers
{
    public class CorporationEventHandler : EventHandlerBase,
        IEventHandler<CorporationCreatedEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public CorporationEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(CorporationCreatedEvent evnt)
        {
            _dbContext.Corporations.Add(new Corporation
            {
                CorporationId = evnt.EntityId,
                CreatedByPlayerId = evnt.OwnerId,
                Name = evnt.Name,
                Description = evnt.Description,
                Credits = 0,
                Warehouse = new Warehouse(),
            });
            return Task.CompletedTask;
        }
    }
}
