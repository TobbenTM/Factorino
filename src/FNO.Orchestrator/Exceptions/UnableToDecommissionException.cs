using System;

namespace FNO.Orchestrator.Exceptions
{
    public class UnableToDecommissionException : Exception
    {
        public UnableToDecommissionException()
        {
        }

        public UnableToDecommissionException(string message) : base(message)
        {
        }
    }
}
