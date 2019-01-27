using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class FactoryEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldAddFactory()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Creating,
            };

            // Act
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, null));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories.First();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
            }
        }

        [Fact]
        public async Task ShouldUpdateFactoryToStarting()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Starting,
            };

            // Act
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, null));
            await When(new FactoryProvisionedEvent(expectedFactory.FactoryId, null));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories.First();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
                Assert.Equal(expectedFactory.State, factory.State);
            }
        }

        [Fact]
        public async Task ShouldUpdateFactoryToOnline()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Online,
            };

            // Act
            await When(new FactoryCreatedEvent(expectedFactory.FactoryId, null));
            await When(new FactoryProvisionedEvent(expectedFactory.FactoryId, null));
            await When(new FactoryOnlineEvent(expectedFactory.FactoryId));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Factories);
                var factory = dbContext.Factories.First();
                Assert.Equal(expectedFactory.FactoryId, factory.FactoryId);
                Assert.Equal(expectedFactory.State, factory.State);
            }
        }
    }
}
