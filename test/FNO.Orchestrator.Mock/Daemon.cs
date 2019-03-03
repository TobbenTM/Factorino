using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
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

        private readonly Dictionary<Guid, FactoryCreatedEvent> _fakeFactories;

        public Daemon()
        {
            _fakeFactories = new Dictionary<Guid, FactoryCreatedEvent>();
        }

        public void Init(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _configurationModel = configuration.Bind<ConfigurationBase>();
            _consumer = new KafkaConsumer(_configurationModel, this, _logger);
            _producer = new KafkaProducer(_configurationModel, _logger);
        }

        public Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            if (typeof(TEvent) == typeof(FactoryCreatedEvent))
            {
                var factoryCreated = evnt as FactoryCreatedEvent;
                _fakeFactories.Add(factoryCreated.EntityId, factoryCreated);
            }
            if (typeof(TEvent) == typeof(FactoryProvisionedEvent))
            {
                var factoryProvisioned = evnt as FactoryProvisionedEvent;
                if (_fakeFactories.ContainsKey(factoryProvisioned.EntityId))
                {
                    _fakeFactories.Remove(factoryProvisioned.EntityId);
                }
            }
            return Task.CompletedTask;
        }

        public async Task OnEndReached(string topic, int partition, long offset)
        {
            foreach (var factory in _fakeFactories)
            {
                _logger.Information($"Factory created: {factory.Key} but not provisioned, provisioning fake resource in 3 seconds..");
                var response = new FactoryProvisionedEvent(factory.Key, factory.Value.Initiator.ToPlayer());
                await Task.Delay(3000).ContinueWith((_) => _producer.Produce(KafkaTopics.EVENTS, response));
            }
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
