using FNO.Domain.Models;
using FNO.Orchestrator.Models;
using System;
using Xunit;

namespace FNO.Orchestrator.Tests
{
    public class StateTests
    {
        private readonly State _state;

        public StateTests()
        {
            _state = new State();
        }

        [Fact]
        public void StateShouldAddAndGetFactory()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
            };

            // Act
            _state.AddFactory(expectedFactory);
            var result = _state.GetFactory(expectedFactory.FactoryId);

            // Assert
            Assert.Same(expectedFactory, result);
        }

        [Fact]
        public void StateShouldSetAndGetUsername()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedUsername = Guid.NewGuid().ToString();

            // Act
            _state.SetUsername(playerId, expectedUsername);
            var result = _state.GetUsername(playerId);

            // Assert
            Assert.Equal(expectedUsername, result);
        }

        [Fact]
        public void StateShouldReturnNullForMissingUsername()
        {
            // Act
            // Assert
            Assert.Null(_state.GetUsername(Guid.NewGuid()));
        }
    }
}
