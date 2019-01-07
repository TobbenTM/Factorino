using System;

namespace FNO.Domain.Events.Corporation
{
    public class CorporationCreatedEvent : EntityEvent
    {
        public Guid OwnerId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public CorporationCreatedEvent()
        {
        }

        public CorporationCreatedEvent(Models.Corporation corporation, Models.Player initiator) : base(corporation.CorporationId, initiator)
        {
            OwnerId = corporation.CreatedByPlayerId;
            Name = corporation.Name;
            Description = corporation.Description;
        }

        public CorporationCreatedEvent(Guid corporationId, Guid ownerId, string name, string description, Models.Player initiator) : base(corporationId, initiator)
        {
            OwnerId = ownerId;
            Name = name;
            Description = description;
        }
    }
}
