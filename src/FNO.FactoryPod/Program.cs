using Confluent.Kafka;
using FNO.Common;
using FNO.EventStream;
using Serilog;
using System;
using System.Threading;

namespace FNO.FactoryPod
{
    internal class Program : IDisposable
    {
        static void Main(string[] args)
        {
            using (var program = new Program())
            {
                program.Run();
            }
        }

        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly FactoryPodConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly Daemon _daemon;
        private readonly Mediator _mediator;
        private readonly KafkaConsumer _consumer;

        public Program()
        {
            var configuration = Configuration.GetConfiguration();
            _configuration = configuration.Bind<FactoryPodConfiguration>();
            _logger = Logging.GetLogger(configuration);

            // Handle user exit (CTRL + C) gracefully
            Console.CancelKeyPress += new ConsoleCancelEventHandler((_, e) =>
            {
                // Prevent premature app termination
                e.Cancel = true;
                // Allow graceful exit
                _resetEvent.Set();
            });

            // Handle system exit gracefully
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((_, e) => _resetEvent.Set());

            _daemon = new Daemon(_configuration, _logger);
            _mediator = new Mediator(_configuration, _logger);
            _consumer = new KafkaConsumer(_configuration, _mediator, _logger);

            _daemon.OnRconReady += (async (s, e) => await _mediator.Connect());
            _daemon.OnOutputData += (async (s, e) => await _mediator.LogOutput(e));
            _daemon.OnErrorData += (async (s, e) => await _mediator.LogError(e));

            _mediator.OnDisconnect += (s, e) => _resetEvent.Set();
        }

        public void Run()
        {
            _daemon.EnsureServerConfiguration();
            _daemon.EnsureInitialSave();
            _daemon.EnsureModsInstalled();
            _daemon.Run();

            _consumer.Subscribe(new TopicPartitionOffset(KafkaTopics.EVENTS, 0, 0));

            _resetEvent.WaitOne();
        }

        public void Dispose()
        {
            _consumer.Dispose();
            _mediator.Dispose();
            _daemon.Dispose();
        }
    }
}
