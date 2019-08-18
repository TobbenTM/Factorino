using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using CoreRCON;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.EventStream;
using FNO.FactoryPod.Extensions;
using FNO.FactoryPod.Models;
using Newtonsoft.Json;
using Serilog;

namespace FNO.FactoryPod
{
    /// <summary>
    /// Responsible for handling communication between the factorio RCON
    /// interface and the cluster event stream
    /// </summary>
    internal sealed class Mediator : IEventConsumer, IDisposable
    {
        private const int POLL_INTERVAL_MS = 2000;

        private readonly FactoryPodConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Timer _pollTimer;
        private readonly KafkaProducer _producer;
        private readonly State _state;
        private RCON _rcon;

        internal event EventHandler OnDisconnect;

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
            _state = new State();
        }

        private void PollHandler(object sender, ElapsedEventArgs e)
        {
            PollServer().Wait();
        }

        private async Task PollServer()
        {
            _logger.Debug($"Polling factorio server...");

            string result;
            try
            {
                result = await _rcon.SendCommandAsync("/factorino_export");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Could not finish RCON command! {e.Message}");
                return;
            }

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

                await _producer.Produce(KafkaTopics.EVENTS, sysEvents);
            }
        }

        internal Task LogOutput(DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return Task.CompletedTask;
            return _producer.Produce(KafkaTopics.FACTORY_LOGS, new FactoryLogEvent(_configuration.Factorino.FactoryId)
            {
                IsError = false,
                Message = e.Data
            });
        }

        internal Task LogError(DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return Task.CompletedTask;
            return _producer.Produce(KafkaTopics.FACTORY_LOGS, new FactoryLogEvent(_configuration.Factorino.FactoryId)
            {
                IsError = true,
                Message = e.Data
            });
        }

        internal async Task Connect()
        {
            _logger.Debug("Connecting to RCON interface...");

            var settings = _configuration.Factorio.Rcon;
            var address = IPAddress.Parse("127.0.0.1");

            _rcon = new RCON(address, (ushort)settings.Port, settings.Password);
            _rcon.OnDisconnected += () =>
            {
                _logger.Error("RCON interface unexpectedly disconnected!");
                OnDisconnect?.Invoke(this, EventArgs.Empty);
            };

            // await _rcon.ConnectAsync(); // Should be unecessary
            var response = await _rcon.SendCommandAsync("/version");
            _logger.Information($"Got version response: {response}");

            await _producer.Produce(KafkaTopics.EVENTS, new FactoryOnlineEvent(_configuration.Factorino.FactoryId));

            _logger.Information($"Factory Online! Starting RCON poll..");

            _pollTimer.Start();
        }

        public void Dispose()
        {
            _pollTimer.Stop();
            _rcon?.Dispose();
            _producer.Dispose();
        }

        public async Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            if (evnt is ShipmentRequestedEvent shipmentRequestedEvent)
            {
                if (shipmentRequestedEvent.FactoryId == _configuration.Factorino.FactoryId)
                {
                    _logger.Debug($"Shipment requested for factory, waiting for fulfillment...");
                    _state.PendingShipments[shipmentRequestedEvent.EntityId] = new Shipment
                    {
                        ShipmentId = shipmentRequestedEvent.EntityId,
                        DestinationStation = shipmentRequestedEvent.DestinationStation,
                        Carts = shipmentRequestedEvent.Carts,
                        WaitConditions = shipmentRequestedEvent.WaitConditions,
                    };
                }
            }
            else if (evnt is ShipmentFulfilledEvent shipmentFulfilledEvent)
            {
                if (shipmentFulfilledEvent.FactoryId == _configuration.Factorino.FactoryId)
                {
                    _logger.Debug($"Shipment fulfilled, spawning resources and producing event...");
                    if (_state.PendingShipments.ContainsKey(shipmentFulfilledEvent.EntityId))
                    {
                        try
                        {
                            var shipment = _state.PendingShipments[shipmentFulfilledEvent.EntityId];
                            await _rcon.SendCommandAsync($"/factorino_import {JsonConvert.SerializeObject(shipment.ToDTO())}");
                            await _producer.ProduceAsync(new ShipmentReceivedEvent(shipment.ShipmentId, _configuration.Factorino.FactoryId, null));
                            _state.PendingShipments.Remove(shipmentFulfilledEvent.EntityId);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, $"Could not spawn shipment {shipmentFulfilledEvent.EntityId}!");
                        }
                    }
                    else
                    {
                        _logger.Error($"Expected to find shipment with ID {shipmentFulfilledEvent.EntityId} in pending shipments, but could not find it!");
                    }
                }
            }
            else if (evnt is ShipmentReceivedEvent shipmentReceivedEvent)
            {
                if (shipmentReceivedEvent.FactoryId == _configuration.Factorino.FactoryId)
                {
                    _logger.Debug($"Shipment successfully received! Removing from pending shipments...");
                    if (_state.PendingShipments.ContainsKey(shipmentReceivedEvent.EntityId))
                    {
                        _state.PendingShipments.Remove(shipmentReceivedEvent.EntityId);
                    }
                }
            }
        }

        public Task OnEndReached(string topic, int partition, long offset)
        {
            // noop
            return Task.CompletedTask;
        }
    }
}
