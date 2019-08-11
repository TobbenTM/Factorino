using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using FNO.Broker.EventHandlers;
using FNO.Broker.Infrastructure;
using FNO.Broker.Models;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Market;
using FNO.Domain.Events.Player;
using FNO.Domain.Events.Shipping;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FNO.Broker
{
    internal sealed class Daemon : IConsumerDaemon, IEventConsumer
    {
        private ILogger _logger;
        private KafkaConsumer _consumer;
        private KafkaProducer _producer;
        private readonly EventHandlerResolver _resolver;
        private readonly State _state;

        public Daemon()
        {
            _state = new State();
            _resolver = new EventHandlerResolver();
        }

        public void Init(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;

            var configurationModel = configuration.Bind<BrokerConfiguration>();
            _consumer = new KafkaConsumer(configurationModel, this, _logger);
            _producer = new KafkaProducer(configurationModel, _logger);

            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            _resolver.Register(() => new FactoryEventHandler(_state),
                typeof(FactoryCreatedEvent),
                typeof(FactoryOutgoingTrainEvent));

            _resolver.Register(() => new OrderEventHandler(_state),
                typeof(OrderCreatedEvent),
                typeof(OrderCancelledEvent),
                typeof(OrderFulfilledEvent),
                typeof(OrderTransactionEvent));

            _resolver.Register(() => new PlayerEventHandler(_state),
                typeof(PlayerCreatedEvent),
                typeof(PlayerBalanceChangedEvent));

            _resolver.Register(() => new ShipmentEventHandler(_state),
                typeof(ShipmentRequestedEvent),
                typeof(ShipmentFulfilledEvent),
                typeof(ShipmentCompletedEvent));
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var handlers = _resolver.Resolve(evnt);
            if (!handlers.Any())
            {
                _logger.Information($"Skipping event of type {evnt.GetType().FullName}, no handlers registered.");
                return;
            }
            foreach (var handler in handlers)
            {
                await handler.Handle(evnt);
            }
        }

        public async Task OnEndReached(string topic, int partition, long offset)
        {
            var evaluator = new Evaluator(_logger);
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
