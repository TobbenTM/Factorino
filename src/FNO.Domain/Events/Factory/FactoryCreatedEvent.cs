﻿using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryCreatedEvent : EntityEvent
    {
        public Guid LocationId { get; set; }

        public FactoryCreatedEvent()
        {
        }

        public FactoryCreatedEvent(Guid factoryId, Guid locationId, Models.Player initiator) : base(factoryId, initiator)
        {
            LocationId = locationId;
        }
    }
}
