using FNO.Domain.Models;

namespace FNO.Broker.Models
{
    /// <summary>
    /// Variant of the market order optimized for easier warehouse management
    /// </summary>
    public class BrokerOrder : MarketOrder
    {
        public new BrokerPlayer Owner { get; set; }
    }
}
