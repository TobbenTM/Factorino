using FNO.Common;
using FNO.Domain.Events;
using FNO.EventStream;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;

namespace FNO.Toolbox.Common
{
    internal class ProducerDialog : Dialog
    {
        private readonly KafkaProducer _producer;
        private readonly Label _label;

        public ProducerDialog() : base("Producing event..", 0, 0)
        {
            var config = Configuration.GetConfiguration().Bind<ConfigurationBase>();
            var logger = Logging.GetLogger();
            _producer = new KafkaProducer(config, logger);

            Width = 64;
            Height = 9;

            _label = new Label("Connecting...")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 60,
            };
            Add(_label);
        }

        public async Task Produce(IEvent evnt)
        {
            _label.Text = $"Producing {evnt.GetType().Name} to {KafkaTopics.EVENTS}..";
            var result = (await _producer.ProduceAsync(evnt)).Single();
            _label.Text = $"Produced successfully!\nOffset: {result.Offset}, Partition: {result.Partition}, Topic: {result.Topic}";
        }

        public async Task Produce(IEvent[] evnts)
        {
            _label.Text = $"Producing {evnts.Length} events to {KafkaTopics.EVENTS}..";
            var result = (await _producer.ProduceAsync(evnts)).Last();
            _label.Text = $"Produced successfully!\nOffset: {result.Offset}, Partition: {result.Partition}, Topic: {result.Topic}";
        }
    }
}
