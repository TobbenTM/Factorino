using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryCreatedEvent : EntityEvent
    {
        public Guid LocationId { get; set; }
        public string LocationSeed { get; set; }

        public FactoryCreatedEvent()
        {
        }

        public FactoryCreatedEvent(Guid factoryId, Guid locationId, string locationSeed, Models.Player initiator) : base(factoryId, initiator)
        {
            LocationId = locationId;
            LocationSeed = locationSeed;
        }

        public override string ReadableEvent => $"{Initiator?.PlayerName ?? "An unknown entity"} created the factory";
    }
}
