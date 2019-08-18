using System;
using System.Collections.Generic;
using FNO.Domain.Models;

namespace FNO.FactoryPod.Models
{
    public class State
    {
        public Dictionary<Guid, Shipment> PendingShipments { get; } = new Dictionary<Guid, Shipment>();
    }
}
