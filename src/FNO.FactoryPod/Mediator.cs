using CoreRCON;
using FNO.Domain.Events.Factory;
using FNO.EventStream;
using FNO.FactoryPod.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace FNO.FactoryPod
{
    /// <summary>
    /// Responsible for handling communication between the factorio RCON
    /// interface and the cluster event stream
    /// </summary>
    internal class Mediator : IDisposable
    {
        private const int POLL_INTERVAL_MS = 2000;

        private readonly FactoryPodConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Timer _pollTimer;
        private readonly KafkaProducer _producer;

        private RCON _rcon;

        internal Mediator(FactoryPodConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            _pollTimer = new Timer
            {
                Interval = POLL_INTERVAL_MS
            };
            _pollTimer.Elapsed += new ElapsedEventHandler(PollHandler);
            _producer = new KafkaProducer(_configuration, _logger);
        }

        private void PollHandler(object sender, ElapsedEventArgs e)
        {
            PollServer().Wait();
        }

        private async Task PollServer()
        {
            var result = await _rcon.SendCommandAsync("/factorino_export");

            if (result.Length > 2)
            {
                _logger.Debug($"Got payload: {result}");
            }

            PodEventDTO[] events = null;
            try
            {
                events = JsonConvert.DeserializeObject<PodEventDTO[]>(result);
            }
            catch (JsonReaderException e)
            {
                _logger.Error($"Could not deserialize poll response! Error: {e.Message}, raw: {result}");
            }

            if (events != null && events.Length > 0)
            {
                _logger.Information($"Got events: {string.Join<PodEventDTO>(", ", events)}");

                var sysEvents = FactorioEventFactory.TransformEvents(_configuration, events);

                await _producer.Produce(KafkaTopics.FACTORY_ACTIVITY, sysEvents);
            }
        }

        internal Task LogOutput(DataReceivedEventArgs e)
        {
            return _producer.Produce(KafkaTopics.FACTORY_LOGS, new FactoryLogEvent(_configuration.Factorino.FactoryId)
            {
                IsError = false,
                Message = e.Data
            });
        }

        internal Task LogError(DataReceivedEventArgs e)
        {
            return _producer.Produce(KafkaTopics.FACTORY_LOGS, new FactoryLogEvent(_configuration.Factorino.FactoryId)
            {
                IsError = true,
                Message = e.Data
            });
        }

        internal async Task Connect()
        {
            var settings = _configuration.Factorio.Rcon;
            var address = IPAddress.Parse("127.0.0.1");
            _rcon = new RCON(address, (ushort)settings.Port, settings.Password);

            await _rcon.ConnectAsync();
            var response = await _rcon.SendCommandAsync("/version");
            _logger.Information($"Got version response: {response}");

            await _producer.Produce(KafkaTopics.FACTORY_ACTIVITY, new FactoryOnlineEvent(_configuration.Factorino.FactoryId));

            _pollTimer.Start();
        }

        public void Dispose()
        {
            _pollTimer.Stop();
            _rcon?.Dispose();
            _producer.Dispose();
        }
    }
}
