using FNO.Domain.Repositories;
using FNO.EventSourcing;

namespace FNO.WebApp.Hubs
{
    public class ShippingHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IEventStore _eventStore;

        public ShippingHub(
            IPlayerRepository playerRepository,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _eventStore = eventStore;
        }
    }
}
