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
    }
}
