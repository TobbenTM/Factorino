using System;

namespace FNO.FactoryPod.Exceptions
{
    class EventUnknownException : Exception
    {
        public EventUnknownException(string message) : base(message)
        {
        }
    }
}
