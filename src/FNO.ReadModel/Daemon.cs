using Confluent.Kafka;
using FNO.Common;
using FNO.Domain;
using FNO.Domain.Events;
using FNO.Domain.Models;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel
{
    /// <summary>
    /// The readmodel daemon will only consume events not already consumer
    /// for the current readmodel database. This means we need to check
    /// the last consumer offset, and continue from that.
    /// </summary>
    internal class Daemon : IConsumerDaemon, IEventConsumer
    {
        // TODO: Refactor these so they're readonly again
        private IConfiguration _configuration;
        private ConfigurationBase _configurationModel;

        private ILogger _logger;
        private KafkaConsumer _consumer;

        public Daemon()
        {
        }

        public void Init(IConfiguration configuration, ILogger logger)
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

        public Task OnEndReached(string topic, int partition, long offset)
        {
            // noop
            return Task.CompletedTask;
        }

        public void Run()
        {
            using (var dbContext = ReadModelDbContext.CreateContext(_configuration))
            {
                var initialState = dbContext.ConsumerStates
                    .Where(s => s.GroupId == _configurationModel.Kafka.GroupId)
                    .ToList();
                if (initialState.Count() > 0)
                {
                    // We have already rehydrated this context; lets continue where we left off
                    var subscriptions = initialState
                        .Select(s => new TopicPartitionOffset(s.Topic, s.Partition, s.Offset + 1))
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
