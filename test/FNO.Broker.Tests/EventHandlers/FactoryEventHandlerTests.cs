using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Broker.EventHandlers;
using FNO.Broker.Models;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using Xunit;

namespace FNO.Broker.Tests.EventHandlers
{
    public class FactoryEventHandlerTests
    {
        private readonly State _state;
        private readonly FactoryEventHandler _handler;

        public FactoryEventHandlerTests()
        {
            _state = new State();
            _handler = new FactoryEventHandler(_state);
        }

        [Fact]
        public async Task HandlerShouldAddFactoryOwner()
        {
            // Arrange
            var expectedFactoryId = Guid.NewGuid();
            var expectedOwner = new BrokerPlayer
            {
                PlayerId = Guid.NewGuid(),
            };
            _state.Players.Add(expectedOwner.PlayerId, expectedOwner);

            // Act
            await _handler.Handle(new FactoryCreatedEvent(expectedFactoryId, default, default, expectedOwner));

            // Assert
            Assert.Equal(expectedOwner.PlayerId, _state.FactoryOwners[expectedFactoryId].PlayerId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HandlerShouldUpdateInventory(bool receiverHasExistingInventory)
        {
            // Arrange
            var rng = new Random();
            var expectedQuantity = rng.Next();
            var expectedFactoryId = Guid.NewGuid();
            var expectedItem = Guid.NewGuid().ToString();
            var receiver = new BrokerPlayer
            {
                PlayerId = Guid.NewGuid(),
                Inventory = new Dictionary<string, WarehouseInventory>(),
            };
            if (receiverHasExistingInventory)
            {
                receiver.Inventory.Add(expectedItem, new WarehouseInventory());
            }
            _state.FactoryOwners.Add(expectedFactoryId, receiver);

            // Act
            await _handler.Handle(new FactoryOutgoingTrainEvent(expectedFactoryId, default, default)
            {
                Inventory = new[]
                {
                    new LuaItemStack
                    {
                        Name = expectedItem,
                        Count = expectedQuantity,
                    }
                }
            });

            // Assert
            Assert.Equal(expectedQuantity, receiver.Inventory[expectedItem].Quantity);
        }
    }
}
