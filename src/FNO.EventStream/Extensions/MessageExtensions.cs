using Confluent.Kafka;
using FNO.Domain.Models;

namespace FNO.EventStream.Extensions
{
    internal static class MessageExtensions
    {
        internal static EventMetadata ToEventMetadata(this Message<Ignore, string> message)
        {
            return new EventMetadata
            {
                Partition = message.Partition,
                Topic = message.Topic,
                Offset = message.Offset.Value,
            };
        }

        internal static EventMetadata ToEventMetadata(this Message<Null, string> message)
        {
            return new EventMetadata
            {
                Partition = message.Partition,
                Topic = message.Topic,
                Offset = message.Offset.Value,
            };
        }
    }
}
