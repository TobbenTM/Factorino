using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Broker.EventHandlers;
using FNO.Broker.Models;
using FNO.Domain.Events.Market;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using Xunit;

namespace FNO.Broker.Tests.EventHandlers
{
    public class OrderEventHandlerTests
    {
        private readonly State _state;
        private readonly OrderEventHandler _handler;

        public OrderEventHandlerTests()
        {
            _state = new State();
            _handler = new OrderEventHandler(_state);
        }

        [Fact]
        public async Task HandlerShouldAddOrder()
        {
            // Arrange
            var expectedPlayer = new BrokerPlayer { PlayerId = Guid.NewGuid() };
            var expectedOrder = new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid().ToString(),
            };
            _state.Players.Add(expectedPlayer.PlayerId, expectedPlayer);

            // Act
            await _handler.Handle(new OrderCreatedEvent(expectedOrder.OrderId, expectedPlayer)
            {
                ItemId = expectedOrder.ItemId,
            });

            // Assert
            Assert.Equal(expectedOrder.OrderId, _state.Orders.Values.Single().OrderId);
            Assert.Equal(expectedOrder.ItemId, _state.Orders.Values.Single().ItemId);
            Assert.Equal(OrderState.Active, _state.Orders.Values.Single().State);
            Assert.Same(expectedPlayer, _state.Orders.Values.Single().Owner);
        }

        [Fact]
        public async Task HandlerShouldFulfillOrder()
        {
            // Arrange
            var initialOrder = new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                State = OrderState.Active,
            };
            _state.Orders.Add(initialOrder.OrderId, initialOrder);

            // Act
            await _handler.Handle(new OrderFulfilledEvent(initialOrder.OrderId, null));

            // Assert
            Assert.Equal(OrderState.Fulfilled, _state.Orders.Values.Single().State);
        }

        [Fact]
        public async Task HandlerShouldCancelOrder()
        {
            // Arrange
            var initialOrder = new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                State = OrderState.Active,
            };
            _state.Orders.Add(initialOrder.OrderId, initialOrder);

            // Act
            await _handler.Handle(new OrderCancelledEvent(initialOrder.OrderId, null));

            // Assert
            Assert.Equal(OrderState.Cancelled, _state.Orders.Values.Single().State);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HandlerShouldHandleTransaction(bool buyerHasExistingInventory)
        {
            // Arrange
            var rng = new Random();
            var expectedPrice = rng.Next();
            var expectedQuantity = rng.Next();
            var expectedItem = Guid.NewGuid().ToString();
            var buyer = new BrokerPlayer
            {
                PlayerId = Guid.NewGuid(),
                Inventory = new Dictionary<string, WarehouseInventory>(),
            };
            if (buyerHasExistingInventory)
            {
                buyer.Inventory.Add(expectedItem, new WarehouseInventory());
            }
            var seller = new BrokerPlayer
            {
                PlayerId = Guid.NewGuid(),
                Inventory = new Dictionary<string, WarehouseInventory>
                {
                    { expectedItem, new WarehouseInventory() }
                }
            };
            var buyOrder = new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                OrderType = OrderType.Buy,
                ItemId = expectedItem,
                Owner = buyer,
            };
            var sellOrder = new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                OrderType = OrderType.Sell,
                ItemId = expectedItem,
                Owner = seller,
            };
            _state.Orders.Add(buyOrder.OrderId, buyOrder);
            _state.Orders.Add(sellOrder.OrderId, sellOrder);

            // Act
            await _handler.Handle(new OrderTransactionEvent(Guid.NewGuid(), null)
            {
                FromSellOrder = sellOrder.OrderId,
                ToBuyOrder = buyOrder.OrderId,
                Quantity = expectedQuantity,
                Price = expectedPrice,
                ItemId = expectedItem,
            });

            // Assert
            Assert.Equal(-expectedPrice, buyer.Credits);
            Assert.Equal(expectedPrice, seller.Credits);
            Assert.Equal(expectedQuantity, buyOrder.QuantityFulfilled);
            Assert.Equal(expectedQuantity, sellOrder.QuantityFulfilled);
            Assert.Equal(expectedQuantity, buyer.Inventory[expectedItem].Quantity);
            Assert.Equal(-expectedQuantity, seller.Inventory[expectedItem].Quantity);
        }

        [Fact]
        public async Task HandlerShouldSkipHandledTransaction()
        {
            // Arrange
            var expectedTransactionId = Guid.NewGuid();
            _state.HandledTransactions.Enqueue(expectedTransactionId);

            // Act
            await _handler.Handle(new OrderTransactionEvent(expectedTransactionId, null));

            // Assert
            Assert.Empty(_state.HandledTransactions);
        }
    }
}
