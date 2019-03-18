using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryDecommissionedEvent : EntityEvent
    {
        public FactoryDecommissionedEvent()
        {
        }

        public FactoryDecommissionedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }

        public override string ReadableEvent => $"{Initiator?.PlayerName ?? "An unknown entity"} decommissioned the factory";
    }
}
