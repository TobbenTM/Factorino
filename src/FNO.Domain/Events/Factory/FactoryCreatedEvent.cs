using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryCreatedEvent : EntityEvent
    {
        public Guid DeedId { get; set; }

        public FactoryCreatedEvent()
        {
        }

        public FactoryCreatedEvent(
            Guid factoryId,
            Guid deedId,
            Models.Player initiator) : base(factoryId, initiator)
        {
            DeedId = deedId;
        }

        public override string ReadableEvent => $"{Initiator?.PlayerName ?? "An unknown entity"} created the factory";
    }
}
