using FNO.Domain;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel.EventHandlers
{
    public sealed class FactoryEventHandler : EventHandlerBase,
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryProvisionedEvent>,
        IEventHandler<FactoryOnlineEvent>,
        IEventHandler<FactoryDestroyedEvent>,
        IEventHandler<FactoryDecommissionedEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(FactoryCreatedEvent evnt)
        {
            _dbContext.Factories.Add(new Factory
            {
                FactoryId = evnt.EntityId,
                OwnerId = evnt.Initiator.PlayerId,
                State = FactoryState.Creating,
                LocationId = evnt.LocationId,
                Seed = evnt.LocationSeed,
                TrainStations = new[] { "Factorino - Dropoff 1" },
            });
            return Task.CompletedTask;
        }

        public Task Handle(FactoryProvisionedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Starting;
                factory.Port = evnt.Port;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryOnlineEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Online;
                factory.LastSeen = evnt.Metadata.CreatedAt ?? factory.LastSeen;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryDestroyedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Destroying;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryDecommissionedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Destroyed;
            }
            return Task.CompletedTask;
        }
    }
}
