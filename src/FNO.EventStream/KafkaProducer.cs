using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FNO.Common;
using FNO.Domain.Events;
using FNO.Domain.Models;
using FNO.EventSourcing;
using FNO.EventStream.Extensions;
using FNO.EventStream.Serialization;
using Serilog;

namespace FNO.EventStream
{
    public sealed class KafkaProducer : IEventStore, IDisposable
    {
        private readonly Producer<Null, string> _producer;
        private readonly ILogger _logger;

        public KafkaProducer(ConfigurationBase configuration, ILogger logger)
        {
            _logger = logger;

            var config = new KafkaSettingsFactory()
                .WithConfiguration(configuration.Kafka)
                .Build();

            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }

        /// <summary>
        /// Will produce to the Kafka default persistent topic and wait for delivery reports
        /// </summary>
        /// <param name="events">The events to produce</param>
        /// <returns>Delivery reports for the produced events</returns>
        public Task<EventMetadata[]> ProduceAsync(params IEvent[] events)
        {
            return ProduceAsync(KafkaTopics.EVENTS, events);
        }

        /// <summary>
        /// Will produce to Kafka and wait for delivery reports
        /// </summary>
        /// <param name="topic">The topic to produce to</param>
        /// <param name="events">The events to produce</param>
        /// <returns>Delivery reports for the produced events</returns>
        public async Task<EventMetadata[]> ProduceAsync(string topic, params IEvent[] events)
        {
            _logger.Information($"Producing {events.Length} events to {topic}..");
            var sw = Stopwatch.StartNew();
            var tasks = new List<Task<Message<Null, string>>>();
            foreach (var evnt in events)
            {
                var content = JsonConverterExtensions.SerializeEvent(evnt);
                tasks.Add(_producer.ProduceAsync(topic, null, content));
            }
            var result = await Task.WhenAll(tasks.ToArray());
            _logger.Information($"Done producing {events.Length} in {sw.ElapsedMilliseconds} ms!");
            sw.Stop();
            return result.Select(m => m.ToEventMetadata()).ToArray();
        }

        /// <summary>
        /// A fire-and-forget way of producing, will return as soon as produce job has been started
        /// </summary>
        /// <param name="topic">The topic to produce to</param>
        /// <param name="events">The events to produce</param>
        /// <returns>Completed task as soon as the produce job has been started</returns>
        public Task Produce(string topic, params IEvent[] events)
        {
            Task.Factory.StartNew(() => ProduceAsync(topic, events));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.Information("Flushing producer..");
            _producer.Flush(TimeSpan.FromSeconds(5));
            _logger.Information("Done flushing producer!");
            _producer.Dispose();
        }
    }
}
