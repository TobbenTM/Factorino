using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Threading.Tasks;

namespace FNO.Orchestrator.Mock
{
    internal class Daemon : IConsumerDaemon, IEventConsumer
    {
        // TODO: Refactor these so they're readonly again
        private IConfiguration _configuration;
        private ConfigurationBase _configurationModel;

        private ILogger _logger;
        private KafkaConsumer _consumer;
        private KafkaProducer _producer;

        public Daemon()
        {
        }

        public void Init(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _configurationModel = configuration.Bind<ConfigurationBase>();
            _consumer = new KafkaConsumer(_configurationModel, this, _logger);
            _producer = new KafkaProducer(_configurationModel, _logger);
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            if (typeof(TEvent) == typeof(FactoryCreatedEvent))
            {
                var factoryCreated = evnt as FactoryCreatedEvent;
                _logger.Information($"Factory created: {factoryCreated.EntityId}, provisioning fake resource in 3 seconds..");
                var response = new FactoryProvisionedEvent(factoryCreated.EntityId, factoryCreated.Initiator.ToPlayer());
                await Task.Delay(3000).ContinueWith((_) => _producer.Produce(KafkaTopics.EVENTS, response));
            }
        }

        public void OnEndReached(string topic, int partition, long offset)
        {
            // noop
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
