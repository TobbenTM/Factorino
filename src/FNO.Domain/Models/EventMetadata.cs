namespace FNO.Domain.Models
{
    public class EventMetadata
    {
        public int Partition { get; set; }
        public long Offset { get; set; }
        public string Topic { get; set; }

        public override string ToString()
        {
            return $"Topic: {Topic}, {Partition}@{Offset}";
        }
    }
}
