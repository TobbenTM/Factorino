using FNO.Common;
using FNO.Domain.Events;
using FNO.EventStream;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;

namespace FNO.Toolbox.ProduceEvent
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
            Debug.WriteLine("Producing event...");
            var result = (await _producer.ProduceAsync(evnt)).Single();
            Debug.WriteLine("Done producing event!");
            _label.Text = $"Produced successfully!\nOffset: {result.Offset}, Partition: {result.Partition}, Topic: {result.Topic}";
        }
    }
}
