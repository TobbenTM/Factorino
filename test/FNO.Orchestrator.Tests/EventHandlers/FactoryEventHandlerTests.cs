using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.Orchestrator.EventHandlers;
using FNO.Orchestrator.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.Orchestrator.Tests
{
    public class FactoryEventHandlerTests
    {
        private readonly State _state;
        private readonly FactoryEventHandler _handler;

        public FactoryEventHandlerTests()
        {
            _state = new State();
            _handler = new FactoryEventHandler(_state);
        }

        [Fact]
        public async Task HandlerShouldAddFactory()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Creating,
            };
            var initiator = new Player { PlayerId = Guid.NewGuid() };
            var expectedUsername = Guid.NewGuid().ToString();
            _state.SetUsername(initiator.PlayerId, expectedUsername);

            // Act
            await _handler.Handle(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.DeedId, initiator));

            // Assert
            Assert.Single(_state.Factories);
            Assert.Equal(expectedFactory.FactoryId, _state.Factories.Single().FactoryId);
            Assert.Equal(expectedFactory.State, _state.Factories.Single().State);
            Assert.Equal(expectedUsername, _state.Factories.Single().OwnerFactorioUsername);
        }

        [Fact]
        public async Task HandlerShouldUpdateFactory()
        {
            // Arrange
            var expectedFactory = new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Starting,
            };

            // Act
            await _handler.Handle(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.DeedId, null));
            await _handler.Handle(new FactoryProvisionedEvent(expectedFactory.FactoryId, null));

            // Assert
            Assert.Single(_state.Factories);
            Assert.Equal(expectedFactory.FactoryId, _state.Factories.Single().FactoryId);
            Assert.Equal(expectedFactory.State, _state.Factories.Single().State);
        }
    }
}
