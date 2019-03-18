using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryDestroyedEvent : EntityEvent
    {
        public FactoryDestroyedEvent()
        {
        }

        public FactoryDestroyedEvent(Guid factoryId, Models.Player initiator) : base(factoryId, initiator)
        {
        }

        public override string ReadableEvent => $"{Initiator?.PlayerName ?? "An unknown entity"} destroyed the factory";
    }
}
