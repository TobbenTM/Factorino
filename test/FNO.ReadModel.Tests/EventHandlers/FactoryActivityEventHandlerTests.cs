using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class FactoryActivityEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldUpdateFactoryResearchToResearching()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                CurrentlyResearchingId = Guid.NewGuid().ToString(),
            };

            // Act
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.LocationId, "seed", null));
            await When(new FactoryResearchStartedEvent(expectedFactory.FactoryId, null, 0)
            {
                Technology = new LuaTechnology
                {
                    Name = expectedFactory.CurrentlyResearchingId,
                },
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories.First();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
                Assert.Equal(expectedFactory.CurrentlyResearchingId, factory.CurrentlyResearchingId);
            }
        }

        [Fact]
        public async Task ShouldUpdateFactoryResearchToDone()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                CurrentlyResearchingId = Guid.NewGuid().ToString(),
            };

            // Act
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.LocationId, "seed", null));
            await When(new FactoryResearchFinishedEvent(expectedFactory.FactoryId, null, 0));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories.First();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
                Assert.Null(factory.CurrentlyResearchingId);
            }
        }

        [Fact]
        public async Task ShouldUpdateFactoryOwnerInventoryWithTrainCargo()
        {
            // Arrange
            var expectedOwner = new Player
            {
                PlayerId = Guid.NewGuid(),
            };
            var expectedFactory = new Factory
            {
                OwnerId = expectedOwner.PlayerId,
                FactoryId = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                CurrentlyResearchingId = Guid.NewGuid().ToString(),
            };
            var expectedItem = new LuaItemStack
            {
                Name = "Test item",
                Count = 123,
            };

            // Act
            await When(new PlayerCreatedEvent(expectedOwner));
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.LocationId, "seed", expectedOwner));
            await When(new FactoryOutgoingTrainEvent(expectedFactory.FactoryId, null, 0)
            {
                Inventory = new[] { expectedItem },
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories
                    .Include(f => f.Owner)
                    .ThenInclude(p => p.WarehouseInventory)
                    .Single();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
                Assert.NotNull(factory.Owner);
                var owner = factory.Owner;
                Assert.Single(owner.WarehouseInventory);
                Assert.Equal(expectedItem.Name, owner.WarehouseInventory.Single().ItemId);
                Assert.Equal(expectedItem.Count, owner.WarehouseInventory.Single().Quantity);
            }
        }
    }
}
