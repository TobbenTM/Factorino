using FNO.Domain.Models;

namespace FNO.Broker.Models
{
    /// <summary>
    /// Variant of the shipment optimized for easier warehouse management
    /// </summary>
    public class BrokerShipment : Shipment
    {
        public new BrokerPlayer Owner { get; set; }
    }
}
