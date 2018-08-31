namespace FNO.Domain.Models
{
    public class EventMetadata
    {
        public int Partition { get; set; }
        public long Offset { get; set; }
        public string Topic { get; set; }
    }
}
