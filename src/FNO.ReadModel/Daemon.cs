using Confluent.Kafka;
using FNO.Common;
using FNO.Domain;
using FNO.Domain.Events;
using FNO.Domain.Models;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel
{
    internal class Daemon : IEventConsumer, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigurationBase _configurationModel;

        private readonly ILogger _logger;
        private readonly KafkaConsumer _consumer;

        internal Daemon(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _configurationModel = configuration.Bind<ConfigurationBase>();
            _consumer = new KafkaConsumer(_configurationModel, this, _logger);
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            using (var dbContext = ReadModelDbContext.CreateContext(_configuration))
            {
                var dispatcher = new EventDispatcher(dbContext, _logger);
                await dispatcher.Handle(evnt);
                UpdateState(dbContext, evnt);
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Could not commit changes from dispatcher!");
                }
            }
        }

        public void Run()
        {
            using (var dbContext = ReadModelDbContext.CreateContext(_configuration))
            {
                var initialState = dbContext.ConsumerStates.Where(s => s.GroupId == _configurationModel.Kafka.GroupId);
                if (initialState.Count() > 0)
                {
                    // We have already rehydrated this context; lets continue where we left off
                    var subscriptions = initialState
                        .Select(s => new TopicPartitionOffset(s.Topic, s.Partition, s.Offset))
                        .ToArray();
                    _logger.Information($"Found existing context state, subscribing to: ${string.Join(", ", subscriptions.Select(s => s.ToString()))}");
                    _consumer.Subscribe(subscriptions);
                }
                else
                {
                    // This context needs rehydration; start from beginning
                    _logger.Information("No existing state found for context; rehydrating..");
                    _consumer.Subscribe(new TopicPartitionOffset(KafkaTopics.EVENTS, 0, 0));
                }
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }

        private void UpdateState(ReadModelDbContext dbContext, IEvent evnt)
        {
            var existingState = dbContext.ConsumerStates.FirstOrDefault(s =>
                s.GroupId == _configurationModel.Kafka.GroupId &&
                s.Topic == evnt.GetMetadata().Topic &&
                s.Partition == evnt.GetMetadata().Partition);

            if (existingState != null)
            {
                // We already have a state; update
                existingState.Offset = evnt.GetMetadata().Offset;
            }
            else
            {
                // We need to start tracking this state
                dbContext.ConsumerStates.Add(new ConsumerState
                {
                    GroupId = _configurationModel.Kafka.GroupId,
                    Topic = evnt.GetMetadata().Topic,
                    Partition = evnt.GetMetadata().Partition,
                    Offset = evnt.GetMetadata().Offset,
                });
            }
        }
    }
}
