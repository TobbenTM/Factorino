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

        public CorporationCreatedEvent(Models.Corporation corporation) : base(corporation.CorporationId)
        {
            OwnerId = corporation.CreatedByPlayerId;
            Name = corporation.Name;
            Description = corporation.Description;
        }

        public CorporationCreatedEvent(Guid corporationId, Guid ownerId, string name, string description) : base(corporationId)
        {
            OwnerId = ownerId;
            Name = name;
            Description = description;
        }
    }
}
