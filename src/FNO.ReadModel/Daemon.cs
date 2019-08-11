using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using FNO.Common;
using FNO.Domain;
using FNO.Domain.Events;
using FNO.Domain.Events.Market;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Domain.Models.Market;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FNO.ReadModel
{
    /// <summary>
    /// The readmodel daemon will only consume events not already consumer
    /// for the current readmodel database. This means we need to check
    /// the last consumer offset, and continue from that.
    /// </summary>
    internal sealed class Daemon : IConsumerDaemon, IEventConsumer
    {
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

        public async Task OnEndReached(string topic, int partition, long offset)
        {
            _logger.Debug($"Reached end of topic '{topic}' (offset: {offset})");

            if (offset == 0 && topic == KafkaTopics.EVENTS)
            {
                _logger.Information("No events found on stream, seeding with data...");

                // If there are no events, we need to seed with events
                var seedEvents = new List<IEvent>();
                var systemId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                var systemPlayer = new Player
                {
                    Name = "Bank of Nauvis",
                    SteamId = "<system>",
                    PlayerId = systemId,
                };

                using (var producer = new KafkaProducer(_configurationModel, _logger))
                {
                    // Create a system player with infinite cash
                    seedEvents.Add(new PlayerCreatedEvent(systemPlayer));
                    seedEvents.Add(new PlayerBalanceChangedEvent(systemId, systemPlayer)
                    {
                        BalanceChange = long.MaxValue,
                    });

                    // Seeding the market with infinite (bad) buy orders
                    var entities = Domain.Seed.EntityLibrary.Data().Select(e => e.Name);
                    var orderIds = Enumerable.Range(0, entities.Count())
                        .Select(n => Guid.Parse($"00000000-0000-0000-1111-{n.ToString().PadLeft(12, '0')}"))
                        .ToArray();
                    var orders = entities.Select((entity, i) => new OrderCreatedEvent(orderIds[i], systemPlayer)
                        {
                            ItemId = entity,
                            OwnerId = systemId,
                            OrderType = OrderType.Buy,
                            Price = 1,
                            Quantity = -1,
                        });
                    seedEvents.AddRange(orders);

                    await producer.ProduceAsync(KafkaTopics.EVENTS, seedEvents.ToArray());

                    _logger.Information("Done seeding!");
                }
            }
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
