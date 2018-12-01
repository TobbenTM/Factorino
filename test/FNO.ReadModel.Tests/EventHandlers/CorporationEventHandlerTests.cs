using FNO.Domain.Events.Corporation;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class CorporationEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldAddCorporation()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedCorporation = new Corporation
            {
                CreatedByPlayerId = playerId,
                CorporationId = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new CorporationCreatedEvent(expectedCorporation));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Corporations);
                var corporation = dbContext.Corporations.First();
                Assert.Equal(expectedCorporation.CorporationId, corporation.CorporationId);
                Assert.Equal(expectedCorporation.Name, corporation.Name);
                Assert.Equal(expectedCorporation.Description, corporation.Description);
                Assert.Equal(0, corporation.Credits);
            }
        }
    }
}
