using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.EventSourcing;
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
        private readonly IConfiguration _configuration;
        private readonly ConfigurationBase _configurationModel;

        private readonly ILogger _logger;
        private readonly KafkaConsumer _consumer;

        private readonly Dictionary<Type, List<IHubContext<EventHandlerHub, IEventHandlerClient>>> _contexts;

        public EventStreamMediator(
            IConfiguration configuration,
            ILogger logger,
            IHubContext<FactoryCreateHub, IEventHandlerClient> factoryCreateHubContext)
        {
            _configuration = configuration;
            _logger = logger;

            _configurationModel = configuration.Bind<ConfigurationBase>();
            _consumer = new KafkaConsumer(_configurationModel, this, _logger);

            RegisterHubContext(factoryCreateHubContext as IHubContext<EventHandlerHub, IEventHandlerClient>,
                typeof(FactoryCreatedEvent),
                typeof(FactoryOnlineEvent));
        }

        private void RegisterHubContext(IHubContext<EventHandlerHub, IEventHandlerClient> context, params Type[] eventTypes)
        {
            foreach (var eventType in eventTypes)
            {
                if (!_contexts.ContainsKey(eventType))
                {
                    _contexts[eventType] = new List<IHubContext<EventHandlerHub, IEventHandlerClient>>
                    {
                        context,
                    };
                }
                else
                {
                    _contexts[eventType].Add(context);
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
                    await handler.Clients.All.ReceiveEvent(evnt);
                }
            }
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
    }
}
