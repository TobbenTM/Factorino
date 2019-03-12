using System;

namespace FNO.Orchestrator.Exceptions
{
    internal class UnableToDecommissionException : Exception
    {
        public UnableToDecommissionException()
        {
        }

        public UnableToDecommissionException(string message) : base(message)
        {
        }
    }
}
