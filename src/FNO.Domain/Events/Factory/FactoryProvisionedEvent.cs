using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryProvisionedEvent : EntityEvent
    {
        public string ResourceId { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }

        public FactoryProvisionedEvent()
        {
        }

        public FactoryProvisionedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }
    }
}
