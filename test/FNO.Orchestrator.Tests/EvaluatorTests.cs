using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.Orchestrator.Models;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.Orchestrator.Tests
{
    public class EvaluatorTests
    {
        private readonly ILogger _logger;
        private readonly MockProvisioner _provisioner;
        private readonly Evaluator _evaluator;

        public EvaluatorTests()
        {
            _logger = new LoggerConfiguration().CreateLogger();
            _provisioner = new MockProvisioner();
            _evaluator = new Evaluator(_provisioner, _logger);
        }

        [Fact]
        public async Task EvaluatorShouldHaveNoChangesWithNoState()
        {
            // Arrange
            var givenState = new State();

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Empty(result);
            Assert.Empty(_provisioner.FactoriesProvisioned);
        }

        [Fact]
        public async Task EvaluatorShouldHaveNoChangesWithStartedFactories()
        {
            // Arrange
            var givenState = new State();
            givenState.AddFactory(new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Online,
            });
            givenState.AddFactory(new Factory
            {
                FactoryId = Guid.NewGuid(),
                State = FactoryState.Starting,
            });

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Empty(result);
            Assert.Empty(_provisioner.FactoriesProvisioned);
        }

        [Fact]
        public async Task EvaluatorShouldProvisionFactory()
        {
            // Arrange
            var givenState = new State();
            var factoryId = Guid.NewGuid();
            givenState.AddFactory(new Factory
            {
                FactoryId = factoryId,
                State = FactoryState.Creating,
            });

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Single(result);
            Assert.IsType<FactoryProvisionedEvent>(result.Single());

            Assert.Equal(factoryId, (result.Single() as FactoryProvisionedEvent).EntityId);

            Assert.Single(_provisioner.FactoriesProvisioned);
            Assert.Equal(factoryId, _provisioner.FactoriesProvisioned.Single().FactoryId);
        }

        [Fact]
        public async Task EvaluatorShouldOnlyProvisionCreatingFactories()
        {
            // Arrange
            var givenState = new State();
            var factoryIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            givenState.AddFactory(new Factory
            {
                FactoryId = factoryIds[0],
                State = FactoryState.Creating,
            });
            givenState.AddFactory(new Factory
            {
                FactoryId = factoryIds[1],
                State = FactoryState.Online,
            });
            givenState.AddFactory(new Factory
            {
                FactoryId = factoryIds[2],
                State = FactoryState.Creating,
            });

            // Act
            var result = await _evaluator.Evaluate(givenState);

            // Assert
            Assert.Equal(2, result.Count());

            Assert.Equal(factoryIds[0], (result.First() as FactoryProvisionedEvent).EntityId);
            Assert.Equal(factoryIds[2], (result.Last() as FactoryProvisionedEvent).EntityId);

            Assert.Equal(2, _provisioner.FactoriesProvisioned.Count());
            Assert.Equal(factoryIds[0], _provisioner.FactoriesProvisioned.First().FactoryId);
            Assert.Equal(factoryIds[2], _provisioner.FactoriesProvisioned.Last().FactoryId);
        }
    }
}
