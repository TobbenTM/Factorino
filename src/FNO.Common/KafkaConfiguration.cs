namespace FNO.Common
{
    public class KafkaConfiguration
    {
        public string GroupId { get; set; }
        public string[] BootstrapServers { get; set; }
        public bool DebugLogging { get; set; }
    }
}
