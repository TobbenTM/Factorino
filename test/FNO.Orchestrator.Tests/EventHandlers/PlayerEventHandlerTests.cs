using FNO.Domain.Events.Player;
using FNO.Orchestrator.EventHandlers;
using FNO.Orchestrator.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FNO.Orchestrator.Tests.EventHandlers
{
    public class PlayerEventHandlerTests
    {
        private readonly State _state;
        private readonly PlayerEventHandler _handler;

        public PlayerEventHandlerTests()
        {
            _state = new State();
            _handler = new PlayerEventHandler(_state);
        }

        [Fact]
        public async Task HandlerShouldSetUsername()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedUsername = Guid.NewGuid().ToString();

            // Act
            await _handler.Handle(new PlayerFactorioIdChangedEvent(playerId, null) { FactorioId = expectedUsername });

            // Assert
            Assert.Equal(expectedUsername, _state.GetUsername(playerId));
        }
    }
}
