using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FNO.Domain.Events.Market;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authorization;

namespace FNO.WebApp.Hubs
{
    public class MarketHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IEntityRepository _entityRepository;
        private readonly IMarketRepository _repo;
        private readonly IEventStore _eventStore;

        public MarketHub(
            IPlayerRepository playerRepository,
            IEntityRepository entityRepository,
            IMarketRepository repo,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _entityRepository = entityRepository;
            _repo = repo;
            _eventStore = eventStore;
        }

        [Authorize]
        public async Task<IEnumerable<MarketOrder>> GetOrders()
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var orders = await _repo.GetOrders(player);
            foreach (var order in orders)
            {
                await Subscribe(order.OrderId);
            }
            return orders;
        }

        [Authorize]
        public async Task<MarketOrder> CreateOrder(MarketOrder order)
        {
            var player = await _playerRepo.GetPlayer(Context.User);

            var orderId = Guid.NewGuid();

            order.OrderId = orderId;
            order.Item = _entityRepository.Get(order.ItemId);

            // We need to receive all events for this order from now on
            await Subscribe(orderId);

            var evnt = new OrderCreatedEvent(orderId, player)
            {
                ItemId = order.ItemId,
                OrderType = order.OrderType,
                Price = order.Price,
                Quantity = order.Quantity,
            };
            var results = await _eventStore.ProduceAsync(evnt);
            return order;
        }

        [Authorize]
        public async Task CancelOrder(Guid orderId)
        {
            var player = await _playerRepo.GetPlayer(Context.User);
            var order = await _repo.GetOrder(orderId);

            if (order.OwnerId != player.PlayerId)
            {
                throw new UnauthorizedAccessException("User does not own the order");
            }

            var evnt = new OrderCancelledEvent(orderId, player)
            {
                CancellationReason = OrderCancellationReason.User,
            };
            await _eventStore.ProduceAsync(evnt);
        }
    }
}
