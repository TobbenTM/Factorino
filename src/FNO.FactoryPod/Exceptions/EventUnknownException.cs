using System;

namespace FNO.FactoryPod.Exceptions
{
    public class EventUnknownException : Exception
    {
        public EventUnknownException(string message) : base(message)
        {
        }
    }
}
