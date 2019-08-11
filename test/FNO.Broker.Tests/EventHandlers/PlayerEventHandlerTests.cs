using FNO.Broker.EventHandlers;
using FNO.Broker.Models;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.Broker.Tests.EventHandlers
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
        public async Task HandlerShouldAddPlayer()
        {
            // Arrange
            var expectedPlayer = new Player
            {
                PlayerId = Guid.NewGuid(),
            };

            // Act
            await _handler.Handle(new PlayerCreatedEvent(expectedPlayer));

            // Assert
            Assert.Equal(expectedPlayer.PlayerId, _state.Players.Values.Single().PlayerId);
            Assert.NotNull(_state.Players.Values.Single().Inventory);
        }

        [Fact]
        public async Task HandlerShouldUpdateBalance()
        {
            // Arrange
            var expectedBalance = new Random().Next();
            var initialPlayer = new BrokerPlayer
            {
                PlayerId = Guid.NewGuid(),
            };
            _state.Players.Add(initialPlayer.PlayerId, initialPlayer);

            // Act
            await _handler.Handle(new PlayerBalanceChangedEvent(initialPlayer.PlayerId, null)
            {
                BalanceChange = expectedBalance,
            });

            // Assert
            Assert.Equal(expectedBalance, initialPlayer.Credits);
        }
    }
}
