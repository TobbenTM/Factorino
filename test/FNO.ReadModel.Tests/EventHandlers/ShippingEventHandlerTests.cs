using System;
using System.Linq;
using System.Threading.Tasks;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class ShippingEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldAddShipment()
        {
            // Arrange
            Given(DefaultPlayer, DefaultFactory);
            var shipmentId = Guid.NewGuid();
            var expectedDestination = Guid.NewGuid().ToString();
            var expectedCargo = new[]
            {
                new Cart
                {
                    CartType = CartType.Cargo,
                    Inventory = new[]
                    {
                        new LuaItemStack
                        {
                            Count = new Random().Next(),
                            Name = Guid.NewGuid().ToString(),
                        },
                    },
                },
            };
            var expectedWaitConditions = new[]
            {
                new WaitCondition
                {
                    CompareType = WaitConditionCompareType.And,
                    Type = WaitConditionType.Empty,
                },
            };

            // Act
            await When(new ShipmentRequestedEvent(shipmentId, _factoryId, DefaultPlayer.Single())
            {
                Carts = expectedCargo,
                WaitConditions = expectedWaitConditions,
                DestinationStation = expectedDestination,
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Shipments);
                var shipment = dbContext.Shipments
                    .Include(s => s.Owner)
                    .Include(s => s.Factory)
                    .Single();
                Assert.Equal(shipmentId, shipment.ShipmentId);
                Assert.Equal(_playerId, shipment.OwnerId);
                Assert.Equal(expectedDestination, shipment.DestinationStation);
                Assert.Equal(ShipmentState.Requested, shipment.State);
                Assert.NotNull(shipment.Owner);
                Assert.NotNull(shipment.Factory);
                Assert.Equal(JsonConvert.SerializeObject(expectedCargo), JsonConvert.SerializeObject(shipment.Carts));
                Assert.Equal(JsonConvert.SerializeObject(expectedWaitConditions), JsonConvert.SerializeObject(shipment.WaitConditions));
            }
        }

        [Fact]
        public async Task ShouldUpdateShipmentWithFulfilled()
        {
            // Arrange
            Given(DefaultPlayer, DefaultFactory);
            var shipmentId = Guid.NewGuid();

            // Act
            await When(new ShipmentRequestedEvent(shipmentId, _factoryId, DefaultPlayer.Single()));
            await When(new ShipmentFulfilledEvent(shipmentId, _factoryId, DefaultPlayer.Single()));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Shipments);
                var shipment = dbContext.Shipments.Single();
                Assert.Equal(ShipmentState.Fulfilled, shipment.State);
            }
        }

        [Fact]
        public async Task ShouldUpdateShipmentWithReceived()
        {
            // Arrange
            Given(DefaultPlayer, DefaultFactory);
            var shipmentId = Guid.NewGuid();

            // Act
            await When(new ShipmentRequestedEvent(shipmentId, _factoryId, DefaultPlayer.Single()));
            await When(new ShipmentReceivedEvent(shipmentId, _factoryId, DefaultPlayer.Single()));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Shipments);
                var shipment = dbContext.Shipments.Single();
                Assert.Equal(ShipmentState.Received, shipment.State);
            }
        }

        [Fact]
        public async Task ShouldUpdateShipmentWithCompleted()
        {
            // Arrange
            Given(DefaultPlayer, DefaultFactory);
            var shipmentId = Guid.NewGuid();

            // Act
            await When(new ShipmentRequestedEvent(shipmentId, _factoryId, DefaultPlayer.Single()));
            await When(new ShipmentCompletedEvent(shipmentId, _factoryId, DefaultPlayer.Single()));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Shipments);
                var shipment = dbContext.Shipments.Single();
                Assert.Equal(ShipmentState.Completed, shipment.State);
            }
        }
    }
}
