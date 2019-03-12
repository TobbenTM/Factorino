using FNO.Domain;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel.EventHandlers
{
    public sealed class FactoryEventHandler : EventHandlerBase,
        IEventHandler<FactoryCreatedEvent>,
        IEventHandler<FactoryProvisionedEvent>,
        IEventHandler<FactoryOnlineEvent>,
        IEventHandler<FactoryOutgoingTrainEvent>,
        IEventHandler<FactoryResearchStartedEvent>,
        IEventHandler<FactoryResearchFinishedEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public FactoryEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
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

        public Task Handle(FactoryProvisionedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.State = FactoryState.Starting;
            }
            return Task.CompletedTask;
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
            });
            return Task.CompletedTask;
        }

        public Task Handle(FactoryOutgoingTrainEvent evnt)
        {
            var factory = _dbContext.Factories
                .Include(f => f.Owner)
                .ThenInclude(p => p.WarehouseInventory)
                .FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null && factory.Owner != null)
            {
                foreach (var item in evnt.Inventory)
                {
                    var existingInventory = factory.Owner.WarehouseInventory
                        .FirstOrDefault(i => i.ItemId == item.Name);

                    if (existingInventory != null)
                    {
                        existingInventory.Quantity += item.Count;
                    }
                    else
                    {
                        factory.Owner.WarehouseInventory.Add(new WarehouseInventory
                        {
                            ItemId = item.Name,
                            Quantity = item.Count,
                        });
                    }
                }
                factory.LastSeen = evnt.Metadata.CreatedAt ?? factory.LastSeen;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryResearchStartedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.CurrentlyResearchingId = evnt.Technology.Name;
                factory.LastSeen = evnt.Metadata.CreatedAt ?? factory.LastSeen;
            }
            return Task.CompletedTask;
        }

        public Task Handle(FactoryResearchFinishedEvent evnt)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.FactoryId == evnt.EntityId);
            if (factory != null)
            {
                factory.CurrentlyResearchingId = null;
                factory.LastSeen = evnt.Metadata.CreatedAt ?? factory.LastSeen;
            }
            return Task.CompletedTask;
        }
    }
}
