using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.EventSourcing;
using FNO.EventStream;
using FNO.Orchestrator.Docker;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
    internal class Daemon : IConsumerDaemon, IEventConsumer
    {
        // TODO: Refactor these so they're readonly again
        private IConfiguration _configuration;
        private OrchestratorConfiguration _configurationModel;

        private ILogger _logger;
        private IProvisioner _provisioner;
        private KafkaConsumer _consumer;
        private KafkaProducer _producer;
        private readonly State _state;

        public Daemon()
        {
            _state = new State();
        }

        public void Init(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
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
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            if (evnt is FactoryCreatedEvent createdEvent)
            {
                var handler = new EventHandler(_state, _logger);
                await handler.Handle(createdEvent);
            }
            else if (evnt is FactoryProvisionedEvent provisionedEvent)
            {
                var handler = new EventHandler(_state, _logger);
                await handler.Handle(provisionedEvent);
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
