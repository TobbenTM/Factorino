using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Broker.EventHandlers;
using FNO.Broker.Models;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;
using Xunit;

namespace FNO.Broker.Tests.EventHandlers
{
    public class ShipmentEventHandlerTests
    {
        private readonly State _state;
        private readonly ShipmentEventHandler _handler;

        public ShipmentEventHandlerTests()
        {
            _state = new State();
            _handler = new ShipmentEventHandler(_state);
        }

        [Fact]
        public async Task HandlerShouldAddShipment()
        {
            // Arrange
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedQuantity = new Random().Next();
            var expectedPlayer = new BrokerPlayer { PlayerId = Guid.NewGuid() };
            var expectedShipment = new BrokerShipment
            {
                ShipmentId = Guid.NewGuid(),
                FactoryId = Guid.NewGuid(),
                OwnerId = expectedPlayer.PlayerId,
                WaitConditions = CreateWaitConditions(),
                DestinationStation = Guid.NewGuid().ToString(),
                Carts = CreateCartContents(expectedItemId, expectedQuantity),
            };
            _state.Players.Add(expectedPlayer.PlayerId, expectedPlayer);

            // Act
            await _handler.Handle(new ShipmentRequestedEvent(expectedShipment.ShipmentId, expectedShipment.FactoryId, expectedPlayer)
            {
                Carts = expectedShipment.Carts.ToArray(),
                WaitConditions = expectedShipment.WaitConditions.ToArray(),
                DestinationStation = expectedShipment.DestinationStation,
                OwnerId = expectedShipment.OwnerId,
            });

            // Assert
            Assert.Equal(expectedShipment.ShipmentId, _state.Shipments.Values.Single().ShipmentId);
            Assert.Equal(expectedShipment.FactoryId, _state.Shipments.Values.Single().FactoryId);
            Assert.Equal(ShipmentState.Requested, _state.Shipments.Values.Single().State);
            Assert.Same(expectedPlayer, _state.Shipments.Values.Single().Owner);
        }

        [Fact]
        public async Task HandlerShouldFulfillShipment()
        {
            // Arrange
            var expectedItemId = Guid.NewGuid().ToString();
            var expectedQuantity = new Random().Next();
            var expectedPlayer = new BrokerPlayer { PlayerId = Guid.NewGuid(), Inventory = CreateInventory(expectedItemId, expectedQuantity) };
            var initialShipment = new BrokerShipment
            {
                ShipmentId = Guid.NewGuid(),
                FactoryId = Guid.NewGuid(),
                State = ShipmentState.Requested,
                Owner = expectedPlayer,
                WaitConditions = CreateWaitConditions(),
                DestinationStation = Guid.NewGuid().ToString(),
                Carts = CreateCartContents(expectedItemId, expectedQuantity),
            };
            _state.Shipments.Add(initialShipment.ShipmentId, initialShipment);

            // Act
            await _handler.Handle(new ShipmentFulfilledEvent(initialShipment.ShipmentId, initialShipment.FactoryId, expectedPlayer));

            // Assert
            Assert.Equal(ShipmentState.Fulfilled, initialShipment.State);
            Assert.Equal(0, expectedPlayer.Inventory.Values.Single().Quantity);
        }

        [Fact]
        public async Task HandlerShouldSkipHandledShipment()
        {
            // Arrange
            var expectedShipmentId = Guid.NewGuid();
            _state.HandledShipments.Enqueue(expectedShipmentId);

            // Act
            await _handler.Handle(new ShipmentFulfilledEvent(expectedShipmentId, default, null));

            // Assert
            Assert.Empty(_state.HandledShipments);
        }

        [Fact]
        public async Task HandlerShouldHandleCompletedShipments()
        {
            // Arrange
            var expectedPlayer = new BrokerPlayer { PlayerId = Guid.NewGuid() };
            var initialShipment = new BrokerShipment
            {
                ShipmentId = Guid.NewGuid(),
                FactoryId = Guid.NewGuid(),
                Owner = expectedPlayer,
            };
            _state.Shipments.Add(initialShipment.ShipmentId, initialShipment);

            // Act
            await _handler.Handle(new ShipmentCompletedEvent(initialShipment.ShipmentId, initialShipment.FactoryId, expectedPlayer));

            // Assert
            Assert.Equal(ShipmentState.Completed, initialShipment.State);
        }

        private Cart[] CreateCartContents(string itemId, int quantity)
        {
            return new[]
            {
                new Cart
                {
                    Inventory = new[]
                    {
                        new LuaItemStack
                        {
                            Name = itemId,
                            Count = quantity,
                        },
                    },
                },
            };
        }

        private WaitCondition[] CreateWaitConditions()
        {
            return new WaitCondition[0];
        }

        private Dictionary<string, WarehouseInventory> CreateInventory(string itemId, int quantity)
        {
            return new Dictionary<string, WarehouseInventory>
            {
                { itemId, new WarehouseInventory{ ItemId = itemId, Quantity = quantity } }
            };
        }
    }
}
