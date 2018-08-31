using FNO.Domain.Events;
using System;

namespace FNO.EventSourcing.Exceptions
{
    public class UnhandledEventException : Exception
    {
        public UnhandledEventException(IEvent evnt)
            : base($"No handler found for event type: {evnt.GetType().FullName}!")
        {
        }
    }
}
