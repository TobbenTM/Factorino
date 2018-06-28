using System;
using FNO.Common;
using Serilog;

namespace FNO.EventStream
{
    public class KafkaConsumer : IDisposable
    {
        private ConfigurationBase _configuration;
        private ILogger _logger;

        public KafkaConsumer(ConfigurationBase configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Dispose()
        {
            // TODO
        }
    }
}
