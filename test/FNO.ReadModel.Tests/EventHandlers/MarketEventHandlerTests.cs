using System;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Events.Market;
using FNO.Domain.Models.Market;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class MarketEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldAddOrder()
        {
            // Arrange
            Given(DefaultPlayer);
            var orderId = Guid.NewGuid();
            var rng = new Random();
            var expectedPrice = rng.Next();
            var expectedQuantity = rng.Next();
            var expectedItemId = "iron-ore";

            // Act
            await When(new OrderCreatedEvent(orderId, DefaultPlayer.Single())
            {
                Price = expectedPrice,
                Quantity = expectedQuantity,
                ItemId = expectedItemId,
                OrderType = OrderType.Buy,
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Orders);
                var order = dbContext.Orders.Single();
                Assert.Equal(orderId, order.OrderId);
                Assert.Equal(_playerId, order.OwnerId);
                Assert.Equal(OrderState.Active, order.State);
                Assert.Equal(expectedPrice, order.Price);
                Assert.Equal(expectedQuantity, order.Quantity);
                Assert.Equal(expectedItemId, order.ItemId);
            }
        }

        [Fact]
        public async Task ShouldUpdateOrderWithPartiallyFulfilled()
        {
            // Arrange
            Given(DefaultPlayer);
            var orderId = Guid.NewGuid();

            // Act
            await When(new OrderCreatedEvent(orderId, DefaultPlayer.Single()));
            await When(new OrderPartiallyFulfilledEvent(orderId, DefaultPlayer.Single()));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Orders);
                var order = dbContext.Orders.Single();
                Assert.Equal(OrderState.PartiallyFulfilled, order.State);
            }
        }

        [Fact]
        public async Task ShouldUpdateOrderWithFulfilled()
        {
            // Arrange
            Given(DefaultPlayer);
            var orderId = Guid.NewGuid();

            // Act
            await When(new OrderCreatedEvent(orderId, DefaultPlayer.Single()));
            await When(new OrderFulfilledEvent(orderId, DefaultPlayer.Single()));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Orders);
                var order = dbContext.Orders.Single();
                Assert.Equal(OrderState.Fulfilled, order.State);
            }
        }

        [Fact]
        public async Task ShouldUpdateOrderWithCancelled()
        {
            // Arrange
            Given(DefaultPlayer);
            var orderId = Guid.NewGuid();

            // Act
            await When(new OrderCreatedEvent(orderId, DefaultPlayer.Single()));
            await When(new OrderCancelledEvent(orderId, DefaultPlayer.Single())
            {
                CancellationReason = OrderCancellationReason.NoResources,
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Orders);
                var order = dbContext.Orders.Single();
                Assert.Equal(OrderState.Cancelled, order.State);
                Assert.Equal(OrderCancellationReason.NoResources, order.CancellationReason);
            }
        }
    }
}
