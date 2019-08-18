using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events.Player;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.Domain.Models.Shipping;
using Serilog;
using Serilog.Core;
using Xunit;

namespace FNO.Broker.Tests
{
    public class EvaluatorTests
    {
        private readonly Logger _logger;
        private readonly Evaluator _evaluator;

        public EvaluatorTests()
        {
            _logger = new LoggerConfiguration().CreateLogger();
            _evaluator = new Evaluator(_logger);
        }

        [Fact]
        public async Task EvaluatorShouldHaveNoChangesWithNoState()
        {
            // Arrange
            var givenState = new State();

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task EvaluatorShouldFulfillShipments()
        {
            // Arrange
            var rng = new Random();
            var givenState = new State();
            var expectedQuantity = rng.Next(1, 100);
            var expectedItem = Guid.NewGuid().ToString();
            var owner = new BrokerPlayer
            {
                Inventory = new Dictionary<string, WarehouseInventory>
                {
                    { expectedItem, new WarehouseInventory { Quantity = expectedQuantity } }
                }
            };
            var shipment = new BrokerShipment
            {
                Owner = owner,
                ShipmentId = Guid.NewGuid(),
                FactoryId = Guid.NewGuid(),
                State = ShipmentState.Requested,
                Carts = new[]
                {
                    new Cart
                    {
                        CartType = CartType.Cargo,
                        Inventory = new[]
                        {
                            new LuaItemStack
                            {
                                Name = expectedItem,
                                Count = expectedQuantity,
                            },
                        },
                    },
                },
            };
            givenState.Shipments.Add(shipment.ShipmentId, shipment);

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.NotEmpty(result);

            // Inventory reduced?
            Assert.Equal(0, owner.Inventory[expectedItem].Quantity);
            Assert.Equal(-expectedQuantity, result.OfType<PlayerInventoryChangedEvent>().Single().InventoryChange.Single().Count);

            // Shipment fulfilled?
            var evnt = result.OfType<ShipmentFulfilledEvent>().Single();
            Assert.Equal(shipment.ShipmentId, evnt.EntityId);
            Assert.Equal(shipment.FactoryId, evnt.FactoryId);
        }

        [Fact]
        public async Task EvaluatorShouldMatchOrders()
        {
            // Arrange
            var rng = new Random();
            var givenState = new State();
            var epxectedPrice = rng.Next(1, 100);
            var expectedQuantity = rng.Next(1, 100);
            var expectedItem = Guid.NewGuid().ToString();
            var buyer = new BrokerPlayer { PlayerId = Guid.NewGuid(), Credits = epxectedPrice * expectedQuantity };
            var seller = new BrokerPlayer { PlayerId = Guid.NewGuid(), Inventory = CreateInventory(expectedItem, expectedQuantity) };
            var buyOrder = CreateOrder(OrderType.Buy, expectedItem, buyer, epxectedPrice);
            var sellOrder = CreateOrder(OrderType.Sell, expectedItem, seller, epxectedPrice);
            givenState.Orders.Add(buyOrder.OrderId, buyOrder);
            givenState.Orders.Add(sellOrder.OrderId, sellOrder);

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.NotEmpty(result);

            // Credits transferred?
            Assert.Equal(0, buyer.Credits);
            Assert.Equal(epxectedPrice * expectedQuantity, seller.Credits);

            // Inventory transferred?
            Assert.Equal(0, seller.Inventory.Values.Single().Quantity);
            Assert.Equal(expectedQuantity, buyer.Inventory.Values.Single().Quantity);

            // Order updated?
            Assert.Equal(expectedQuantity, buyOrder.QuantityFulfilled);
            Assert.Equal(expectedQuantity, sellOrder.QuantityFulfilled);
        }

        [Fact]
        public async Task EvaluatorShouldFulfillOrders()
        {
            // Arrange
            var rng = new Random();
            var givenState = new State();
            var epxectedPrice = rng.Next(1, 100);
            var expectedQuantity = rng.Next(1, 100);
            var expectedItem = Guid.NewGuid().ToString();
            var buyer = new BrokerPlayer { PlayerId = Guid.NewGuid(), Credits = epxectedPrice * expectedQuantity };
            var seller = new BrokerPlayer { PlayerId = Guid.NewGuid(), Inventory = CreateInventory(expectedItem, expectedQuantity) };
            var buyOrder = CreateOrder(OrderType.Buy, expectedItem, buyer, epxectedPrice, expectedQuantity);
            var sellOrder = CreateOrder(OrderType.Sell, expectedItem, seller, epxectedPrice, expectedQuantity);
            givenState.Orders.Add(buyOrder.OrderId, buyOrder);
            givenState.Orders.Add(sellOrder.OrderId, sellOrder);

            // Act
            await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Equal(OrderState.Fulfilled, buyOrder.State);
            Assert.Equal(OrderState.Fulfilled, sellOrder.State);
        }

        private BrokerOrder CreateOrder(OrderType type, string itemId, BrokerPlayer owner, int price, int quantity = -1)
        {
            return new BrokerOrder
            {
                OrderId = Guid.NewGuid(),
                OrderType = type,
                ItemId = itemId,
                Owner = owner,
                Quantity = quantity,
                Price = price,
                State = OrderState.Active,
            };
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
