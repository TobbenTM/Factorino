using FNO.Common;
using System.Collections.Generic;

namespace FNO.EventStream
{
    public class KafkaSettingsFactory
    {
        private readonly IDictionary<string, object> _settings;

        public KafkaSettingsFactory()
        {
            _settings = new Dictionary<string, object>
            {
                { "auto.commit.interval.ms", 5000 },
                { "auto.offset.reset", "earliest" },
            };
        }

        public KafkaSettingsFactory WithConfiguration(KafkaConfiguration conf)
        {
            return WithGroupId(conf.GroupId).WithBootstrapServers(conf.BootstrapServers);
        }

        public KafkaSettingsFactory WithGroupId(string groupId)
        {
            _settings["group.id"] = groupId;
            return this;
        }

        public KafkaSettingsFactory WithBootstrapServer(string server)
        {
            return WithBootstrapServers(server);
        }

        public KafkaSettingsFactory WithBootstrapServers(params string[] servers)
        {
            _settings["bootstrap.servers"] = string.Join(",", servers);
            return this;
        }

        public KafkaSettingsFactory WithDebugLogging()
        {
            _settings["debug"] = "all";
            _settings["log_level"] = 7;
            return this;
        }

        public IDictionary<string, object> Build()
        {
            if (!_settings.ContainsKey("group.id"))
            {
                throw new KeyNotFoundException("Could not build Kafka settings; missing group id!");
            }
            if (!_settings.ContainsKey("bootstrap.servers"))
            {
                throw new KeyNotFoundException("Could not build Kafka settings; missing bootstrap servers!");
            }

            return _settings;
        }
    }
}
