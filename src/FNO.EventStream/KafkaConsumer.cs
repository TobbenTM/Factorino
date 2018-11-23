using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FNO.Common;
using FNO.Domain.Events;
using FNO.EventStream.Extensions;
using FNO.EventStream.Serialization;
using Newtonsoft.Json;
using Serilog;

namespace FNO.EventStream
{
    public class KafkaConsumer : IDisposable
    {
        private readonly ConfigurationBase _configuration;
        private readonly IEventConsumer _handler;
        private readonly ILogger _logger;

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private Consumer<Ignore, string> _consumer;
        private Task _consumeTask;

        public KafkaConsumer(ConfigurationBase configuration, IEventConsumer handler, ILogger logger)
        {
            _configuration = configuration;
            _handler = handler;
            _logger = logger;

            var config = new KafkaSettingsFactory()
                .WithConfiguration(_configuration.Kafka)
                .WithAutoCommit(false)
                .Build();

            _consumer = new Consumer<Ignore, string>(config, null, new StringDeserializer(Encoding.UTF8));

            _consumer.OnError += OnError;
            _consumer.OnConsumeError += OnConsumeError;
            _consumer.OnPartitionsAssigned += OnPartitionsAssigned;
            _consumer.OnPartitionsRevoked += OnPartitionsRevoked;
        }

        private void OnError(object sender, Error e)
        {
            _logger.Error($"[KafkaConsumer] Error! {e}");
        }

        private void OnConsumeError(object sender, Message e)
        {
            _logger.Error($"[KafkaConsumer] Consume Error! {e.Error}");
        }

        private void OnPartitionsAssigned(object sender, List<TopicPartition> e)
        {
            var partitions = e.Select(i => $"Partition: {i.Partition}, Topic: {i.Topic}");
            _logger.Information($"[KafkaConsumer] Partitions assigned: {string.Join(", ", partitions)}");

            if (_consumeTask == null && e.Count > 0)
            {
                _logger.Information("[KafkaConsumer] Starting consume task..");
                _consumeTask = Task.Run(Consume);
            }
        }
        
        private void OnPartitionsRevoked(object sender, List<TopicPartition> e)
        {
            var partitions = e.Select(i => $"Partition: {i.Partition}, Topic: {i.Topic}");
            _logger.Warning($"[KafkaConsumer] Partitions revoked: {string.Join(", ", partitions)}");
        }

        public void Subscribe(params TopicPartitionOffset[] subscriptions)
        {
            var subs = subscriptions.Select(i => $"Partition: {i.Partition}, Topic: {i.Topic}, Offset: {i.Offset}");
            _logger.Information($"Subscribing to the following topic: {string.Join(", ", subs)}..");
            _consumer.Assign(subscriptions);
            _consumeTask = Task.Run(Consume);
        }

        private async Task Consume()
        {
            var token = _cancellationToken.Token;
            var serializerSettings = JsonConverterExtensions.CreateSettings();

            while(!token.IsCancellationRequested)
            {
                if(_consumer.Consume(out var message, 1000))
                {
                    var sw = Stopwatch.StartNew();
                    dynamic evnt = JsonConvert.DeserializeObject(message.Value, serializerSettings);
                    if (evnt as IEvent == null) continue;
                    evnt.Enrich(message.ToEventMetadata());
                    await _handler.HandleEvent(evnt);
                    _logger.Debug($"Processed event {evnt.GetType().FullName} in {sw.ElapsedMilliseconds} ms.");
                }
            }
        }

        public void Dispose()
        {
            _logger.Information("[KafkaConsumer] Disposing Kafka Consumer...");
            if (_consumeTask != null)
            {
                _cancellationToken.Cancel();
                _consumeTask.Wait();
            }
            _consumer.Dispose();
            _logger.Information("[KafkaConsumer] Finished disposing consumer!");
        }
    }
}
