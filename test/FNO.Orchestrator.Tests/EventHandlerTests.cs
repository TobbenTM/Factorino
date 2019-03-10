using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.Orchestrator.Tests
{
    public class EventHandlerTests
    {
        private readonly ILogger _logger;
        private readonly State _state;
        private readonly EventHandler _handler;

        public EventHandlerTests()
        {
            _logger = new LoggerConfiguration().CreateLogger();
            _state = new State();
            _handler = new EventHandler(_state, _logger);
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

            // Act
            await _handler.Handle(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.LocationId, null));

            // Assert
            Assert.Single(_state.Factories);
            Assert.Equal(expectedFactory.FactoryId, _state.Factories.Single().FactoryId);
            Assert.Equal(expectedFactory.State, _state.Factories.Single().State);
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
            await _handler.Handle(new FactoryCreatedEvent(expectedFactory.FactoryId, expectedFactory.LocationId, null));
            await _handler.Handle(new FactoryProvisionedEvent(expectedFactory.FactoryId, null));

            // Assert
            Assert.Single(_state.Factories);
            Assert.Equal(expectedFactory.FactoryId, _state.Factories.Single().FactoryId);
            Assert.Equal(expectedFactory.State, _state.Factories.Single().State);
        }
    }
}
