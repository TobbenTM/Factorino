using Confluent.Kafka;
using FNO.Common;
using FNO.Domain;
using FNO.Domain.Events;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace FNO.ReadModel
{
    internal class Daemon : IEventConsumer, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly KafkaConsumer _consumer;

        internal Daemon(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            _consumer = new KafkaConsumer(configuration.Bind<ConfigurationBase>(), this, _logger);
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            using (var dbContext = ReadModelDbContext.CreateContext(_configuration))
            {
                var dispatcher = new EventDispatcher(dbContext, _logger);
                await dispatcher.Handle(evnt);
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
            _consumer.Subscribe(new TopicPartitionOffset(KafkaTopics.EVENTS, 0, 0));
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
