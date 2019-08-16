using FNO.Domain.Events.Player;
using FNO.EventSourcing;
using FNO.Orchestrator.Models;
using System.Threading.Tasks;

namespace FNO.Orchestrator.EventHandlers
{
    public class PlayerEventHandler : IHandler,
        IEventHandler<PlayerFactorioIdChangedEvent>
    {
        private readonly State _state;

        public PlayerEventHandler(State state)
        {
            _state = state;
        }

        public Task Handle(PlayerFactorioIdChangedEvent evnt)
        {
            _state.SetUsername(evnt.EntityId, evnt.FactorioId);
            return Task.CompletedTask;
        }
    }
}
