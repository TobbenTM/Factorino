using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryLogEvent : EntityEvent
    {
        public FactoryLogEvent(Guid factoryId) : base(factoryId)
        {
        }

        /// <summary>
        /// Basically indicates if the log is in stderr or stdout
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// std* output
        /// </summary>
        public string Message { get; set; }
    }
}
