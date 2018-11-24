using System;

namespace FNO.EventSourcing.Exceptions
{
    public class ConsumerOutOfSyncException : Exception
    {
        public ConsumerOutOfSyncException(string message) : base(message)
        {
        }
    }
}
