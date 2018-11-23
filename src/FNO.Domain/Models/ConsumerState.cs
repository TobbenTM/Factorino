namespace FNO.Domain.Models
{
    public class ConsumerState
    {
        public string GroupId { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public long Offset { get; set; }
    }
}
