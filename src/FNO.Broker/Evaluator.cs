using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FNO.Broker.Models;
using FNO.Domain.Events;
using FNO.Domain.Events.Market;
using FNO.Domain.Events.Player;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Extensions;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.Domain.Models.Shipping;
using Serilog;

namespace FNO.Broker
{
    public class Evaluator
    {
        private readonly ILogger _logger;
        private readonly Player _initiator;

        public Evaluator(ILogger logger)
        {
            _logger = logger;
            _initiator = new Player
            {
                PlayerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Factorino Broker",
            };
        }

        public Task<IEnumerable<IEvent>> Evaluate(State state)
        {
            var changeSet = new List<IEvent>();

            changeSet.AddRange(EvaluateShipments(state));

            changeSet.AddRange(EvaluateOrders(state));

            return Task.FromResult(changeSet.AsEnumerable());
        }

        private IEnumerable<IEvent> EvaluateShipments(State state)
        {
            // We need to handle requested shipments
            var fulfillableShipments = state.Shipments.Values
                .Where(s => s.State == ShipmentState.Requested)
                .ToList();

            foreach (var shipment in fulfillableShipments)
            {
                if (TryUpdateInventory(shipment.Owner, shipment))
                {
                    shipment.State = ShipmentState.Fulfilled;
                    state.HandledShipments.Enqueue(shipment.ShipmentId);
                    yield return new ShipmentFulfilledEvent(shipment.ShipmentId, shipment.FactoryId, _initiator);
                    yield return new PlayerInventoryChangedEvent(shipment.Owner.PlayerId, _initiator)
                    {
                        InventoryChange = shipment.Carts.Reduce(),
                    };
                    _logger.Information($"Fulfilled shipment {shipment.ShipmentId}!");
                }

                // TODO: Should we cancel the shipment?
            }
        }

        private bool TryUpdateInventory(BrokerPlayer player, BrokerShipment shipment)
        {
            var totalInventory = shipment.Carts.Reduce();
            foreach (var stack in totalInventory)
            {
                if (!player.Inventory.ContainsKey(stack.Name) || player.Inventory[stack.Name].Quantity < stack.Count)
                {
                    return false;
                }
            }

            foreach (var stack in totalInventory)
            {
                player.Inventory[stack.Name].Quantity -= stack.Count;
            }

            return true;
        }

        private IEnumerable<IEvent> EvaluateOrders(State state)
        {
            // We need to find fulfillable orders
            var sellOrders = state.Orders.Values
                .Where(o => o.OrderType == OrderType.Sell
                    && o.State == OrderState.Active
                    && o.Owner.Inventory[o.ItemId].Quantity > 0)
                .ToList();

            foreach (var sellOrder in sellOrders)
            {
                var inventory = sellOrder.Owner.Inventory[sellOrder.ItemId];
                var quantityToSell = sellOrder.Quantity == -1 ? inventory.Quantity : Math.Min(sellOrder.Quantity, inventory.Quantity);

                // We need to find buyers while we still have inventory left
                while (quantityToSell > 0)
                {
                    // We want to find the buy order with the highest price that match the sale price
                    var buyOrder = state.Orders.Values
                        .Where(o => o.OrderType == OrderType.Buy
                            && o.State == OrderState.Active
                            && o.ItemId == sellOrder.ItemId
                            && o.Price >= sellOrder.Price
                            && o.Owner.Credits >= sellOrder.Price
                            && o.Owner.PlayerId != sellOrder.Owner.PlayerId)
                        .OrderByDescending(o => o.Price)
                        .FirstOrDefault();
                    if (buyOrder != null)
                    {
                        var evnts = EvaluateBuyOrder(buyOrder, sellOrder, quantityToSell, state);
                        foreach (var evnt in evnts)
                        {
                            yield return evnt;
                        }
                        quantityToSell -= evnts.OfType<OrderTransactionEvent>().Sum(e => e.Quantity);
                    }
                    else
                    {
                        break;
                    }
                }

                if (sellOrder.QuantityFulfilled == sellOrder.Quantity)
                {
                    yield return new OrderFulfilledEvent(sellOrder.OrderId, _initiator);
                    sellOrder.State = OrderState.Fulfilled;
                    _logger.Information($"Sell order ${sellOrder.OrderId} completely fulfilled!");
                }
            }
        }

        private IEnumerable<IEvent> EvaluateBuyOrder(
            BrokerOrder buyOrder,
            BrokerOrder sellOrder,
            long quantityToSell,
            State state)
        {
            // The quantity one order can buy will be the least of either:
            // 1. As much as the buyer can buy (credits)
            // 2. As much as the seller can sell (inventory/order size)
            // 3. As much as the buyer wants to buy (order size)
            if (sellOrder.Price == 0) yield break;
            var affordableQuantity = buyOrder.Owner.Credits / sellOrder.Price;
            var quantityToBuy = Math.Min(affordableQuantity, quantityToSell); // Case 1 & 2
            if (buyOrder.Quantity != -1) // Case 3
            {
                quantityToBuy = Math.Min(quantityToBuy, buyOrder.Quantity - buyOrder.QuantityFulfilled);
            }

            var evnt = new OrderTransactionEvent(Guid.NewGuid(), _initiator)
            {
                FromPlayer = sellOrder.Owner.PlayerId,
                ToPlayer = buyOrder.Owner.PlayerId,
                FromSellOrder = sellOrder.OrderId,
                ToBuyOrder = buyOrder.OrderId,
                ItemId = sellOrder.ItemId,
                Quantity = quantityToBuy,
                Price = quantityToBuy * sellOrder.Price,
            };

            buyOrder.Owner.Credits -= evnt.Price;
            sellOrder.Owner.Credits += evnt.Price;

            buyOrder.QuantityFulfilled += evnt.Quantity;
            sellOrder.QuantityFulfilled += evnt.Quantity;

            UpdatePlayerInventory(buyOrder.Owner, evnt.ItemId, evnt.Quantity);
            UpdatePlayerInventory(sellOrder.Owner, evnt.ItemId, -evnt.Quantity);

            state.HandledTransactions.Enqueue(evnt.EntityId);
            yield return evnt;
            yield return new OrderPartiallyFulfilledEvent(buyOrder.OrderId, _initiator)
            {
                Price = evnt.Price,
                QuantityFulfilled = evnt.Quantity,
            };
            yield return new OrderPartiallyFulfilledEvent(sellOrder.OrderId, _initiator)
            {
                Price = evnt.Price,
                QuantityFulfilled = evnt.Quantity,
            };
            yield return new PlayerBalanceChangedEvent(buyOrder.Owner.PlayerId, _initiator)
            {
                BalanceChange = -evnt.Price,
            };
            yield return new PlayerBalanceChangedEvent(sellOrder.Owner.PlayerId, _initiator)
            {
                BalanceChange = evnt.Price,
            };
            yield return new PlayerInventoryChangedEvent(buyOrder.Owner.PlayerId, _initiator)
            {
                InventoryChange = new[]
                {
                    new LuaItemStack
                    {
                        Name = evnt.ItemId,
                        Count = evnt.Quantity,
                    },
                },
            };
            yield return new PlayerInventoryChangedEvent(sellOrder.Owner.PlayerId, _initiator)
            {
                InventoryChange = new[]
                {
                    new LuaItemStack
                    {
                        Name = evnt.ItemId,
                        Count = -evnt.Quantity,
                    },
                },
            };

            if (buyOrder.QuantityFulfilled == buyOrder.Quantity)
            {
                yield return new OrderFulfilledEvent(buyOrder.OrderId, _initiator);
                buyOrder.State = OrderState.Fulfilled;
                _logger.Information($"Buy order ${buyOrder.OrderId} completely fulfilled!");
            }
        }

        private void UpdatePlayerInventory(BrokerPlayer player, string itemId, long quantity)
        {
            if (player.Inventory.TryGetValue(itemId, out var inventory))
            {
                inventory.Quantity += quantity;
            }
            else
            {
                player.Inventory.Add(itemId, new WarehouseInventory
                {
                    ItemId = itemId,
                    Quantity = quantity,
                });
            }
        }
    }
}
