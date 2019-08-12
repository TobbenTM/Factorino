using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Market;
using FNO.Domain.Events.Player;
using FNO.Domain.Events.Shipping;
using FNO.EventStream;
using FNO.WebApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FNO.WebApp.Services
{
    public class EventStreamMediator : IHostedService, IEventConsumer
    {
        private readonly ILogger _logger;
        private readonly KafkaConsumer _consumer;

        private readonly Dictionary<Type, List<IHubClients<IEventHandlerClient>>> _contexts;

        public EventStreamMediator(
            IConfiguration configuration,
            ILogger logger,
            IHubContext<FactoryHub, IEventHandlerClient> factoryHubContext,
            IHubContext<FactoryCreateHub, IEventHandlerClient> factoryCreateHubContext,
            IHubContext<PlayerHub, IEventHandlerClient> playerHubContext,
            IHubContext<MarketHub, IEventHandlerClient> marketHubContext,
            IHubContext<ShippingHub, IEventHandlerClient> shippingHubContext)
        {
            _logger = logger;

            var configurationModel = configuration.Bind<ConfigurationBase>();
            _consumer = new KafkaConsumer(configurationModel, this, _logger);

            _contexts = new Dictionary<Type, List<IHubClients<IEventHandlerClient>>>();

            RegisterHubContext(factoryCreateHubContext.Clients,
                typeof(FactoryCreatedEvent),
                typeof(FactoryProvisionedEvent),
                typeof(FactoryOnlineEvent));

            RegisterHubContext(factoryHubContext.Clients,
                typeof(FactoryCreatedEvent),
                typeof(FactoryProvisionedEvent),
                typeof(FactoryOnlineEvent),
                typeof(FactoryDestroyedEvent),
                typeof(FactoryDecommissionedEvent));

            RegisterHubContext(factoryHubContext.Clients, GetDecendantsOfClass<FactoryActivityBaseEvent>());

            RegisterHubContext(playerHubContext.Clients,
                typeof(PlayerBalanceChangedEvent),
                typeof(PlayerInventoryChangedEvent));

            RegisterHubContext(marketHubContext.Clients,
                typeof(OrderCreatedEvent),
                typeof(OrderPartiallyFulfilledEvent),
                typeof(OrderFulfilledEvent),
                typeof(OrderCancelledEvent));

            RegisterHubContext(shippingHubContext.Clients,
                typeof(ShipmentCompletedEvent),
                typeof(ShipmentFulfilledEvent),
                typeof(ShipmentReceivedEvent),
                typeof(ShipmentRequestedEvent),
                typeof(FactoryOutgoingTrainEvent));
        }

        private void RegisterHubContext(IHubClients<IEventHandlerClient> clients, params Type[] eventTypes)
        {
            foreach (var eventType in eventTypes)
            {
                if (!_contexts.ContainsKey(eventType))
                {
                    _contexts[eventType] = new List<IHubClients<IEventHandlerClient>>
                    {
                        clients,
                    };
                }
                else
                {
                    _contexts[eventType].Add(clients);
                }
            }
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var eventType = evnt.GetType();
            if (_contexts.ContainsKey(eventType))
            {
                var handlers = _contexts[eventType];
                _logger.Debug($"Found {handlers.Count} handlers for event with type {eventType}, forwarding to hub..");
                foreach (var handler in handlers)
                {
                    // If the event is attached to an entity, we'll forward it
                    // to the specific groups that has subscribed to the entity
                    if (evnt is EntityEvent entityEvent)
                    {
                        await handler.Group(entityEvent.EntityId.ToString()).ReceiveEvent(evnt, evnt.GetType().Name);
                    }
                    else
                    {
                        await handler.All.ReceiveEvent(evnt, evnt.GetType().Name);
                    }
                }
            }
        }

        public Task OnEndReached(string topic, int partition, long offset)
        {
            // noop
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(
                new TopicPartitionOffset(KafkaTopics.EVENTS, 0, Offset.End)
            );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }

        private static Type[] GetDecendantsOfClass<T>() where T : class
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                .ToArray();
        }
    }
}
