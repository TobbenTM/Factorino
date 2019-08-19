using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Player;
using FNO.EventSourcing;
using FNO.EventStream;
using FNO.Orchestrator.Docker;
using FNO.Orchestrator.EventHandlers;
using FNO.Orchestrator.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    internal sealed class Daemon : IConsumerDaemon, IEventConsumer
    {
        private OrchestratorConfiguration _configurationModel;

        private ILogger _logger;
        private IProvisioner _provisioner;
        private KafkaConsumer _consumer;
        private KafkaProducer _producer;
        private readonly State _state;
        private readonly EventHandlerResolver _resolver;

        public Daemon()
        {
            _state = new State();
            _resolver = new EventHandlerResolver();
        }

        public void Init(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;

            _configurationModel = configuration.Bind<OrchestratorConfiguration>();
            _consumer = new KafkaConsumer(_configurationModel, this, _logger);
            _producer = new KafkaProducer(_configurationModel, _logger);

            switch (_configurationModel.Provisioner.Provider)
            {
                case "docker":
                    _provisioner = new DockerProvisioner(_configurationModel.Provisioner.Docker, _logger);
                    break;
            }

            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            _resolver.Register(() => new FactoryEventHandler(_state),
                typeof(FactoryCreatedEvent),
                typeof(FactoryProvisionedEvent),
                typeof(FactoryDestroyedEvent),
                typeof(FactoryDecommissionedEvent));

            _resolver.Register(() => new PlayerEventHandler(_state), typeof(PlayerFactorioIdChangedEvent));
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var handlers = _resolver.Resolve(evnt);
            if (!handlers.Any())
            {
                _logger.Debug($"Skipping event of type {evnt.GetType().FullName}, no handlers registered.");
                return;
            }
            foreach (var handler in handlers)
            {
                await handler.Handle(evnt);
            }
        }

        public async Task OnEndReached(string topic, int partition, long offset)
        {
            var evaluator = new Evaluator(_provisioner, _logger);
            var events = await evaluator.Evaluate(_state);
            await _producer.ProduceAsync(KafkaTopics.EVENTS, events.ToArray());
        }

        public void Run()
        {
            _logger.Information("Starting consumer at offset 0...");
            _consumer.Subscribe(new[] { new TopicPartitionOffset(KafkaTopics.EVENTS, 0, 0) });
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
