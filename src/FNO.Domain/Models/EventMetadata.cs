namespace FNO.Domain.Models
{
    public class EventMetadata
    {
        public int Partition { get; set; }
        public long Offset { get; set; }
        public string Topic { get; set; }
        public long? CreatedAt { get; set; }
        public long? ConsumedAt { get; set; }
        public string SourceAssembly { get; set; }

        public override string ToString()
        {
            return $"Topic: {Topic}, {Partition}@{Offset}";
        }

        public void Enrich(EventMetadata metadata)
        {
            Partition = metadata.Partition;
            Offset = metadata.Offset;
            Topic = metadata.Topic;
            CreatedAt = CreatedAt ?? metadata.CreatedAt;
            ConsumedAt = ConsumedAt ?? metadata.ConsumedAt;
        }
    }
}
