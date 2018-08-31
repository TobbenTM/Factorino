using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FNO.Domain.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using FNO.EventStream.Serialization;
using System.Collections.Generic;
using FNO.Common;
using Serilog;
using FNO.EventSourcing;
using FNO.Domain.Models;
using System.Linq;
using FNO.EventStream.Extensions;

namespace FNO.EventStream
{
    public class KafkaProducer : IEventStore, IDisposable
    {
        private readonly Producer<Null, string> _producer;
        private readonly ConfigurationBase _configuration;
        private readonly ILogger _logger;

        public KafkaProducer(ConfigurationBase configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            var config = new KafkaSettingsFactory()
                .WithConfiguration(_configuration.Kafka)
                .Build();

            _producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
        }

        /// <summary>
        /// Will produce to the Kafka default persistent topic and wait for delivery reports
        /// </summary>
        /// <param name="topic">The topic to produce to</param>
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
            List<Task<Message<Null, string>>> tasks = new List<Task<Message<Null, string>>>();
            foreach (var evnt in events)
            {
                var content = JsonConverterExtensions.SerializeEvent(evnt);
                tasks.Add(_producer.ProduceAsync(topic, null, content));
            }
            var result = await Task.WhenAll(tasks.ToArray());
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
