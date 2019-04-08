using FNO.Domain.Repositories;
using FNO.EventSourcing;

namespace FNO.WebApp.Hubs
{
    public class PlayerHub : EventHandlerHub
    {
        private readonly IPlayerRepository _playerRepo;
        private readonly IEventStore _eventStore;

        public PlayerHub(
            IPlayerRepository playerRepository,
            IEventStore eventStore)
        {
            _playerRepo = playerRepository;
            _eventStore = eventStore;
        }
    }
}
