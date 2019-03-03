using FNO.Common;
using FNO.Domain.Events;
using FNO.EventSourcing;
using FNO.EventStream;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Threading.Tasks;

namespace FNO.Orchestrator
{
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

        public Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            return Task.CompletedTask;
        }

        public void OnEndReached(string topic, int partition, long offset)
        {
            // noop
            return Task.CompletedTask;
        }

        public void Run()
        {
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
