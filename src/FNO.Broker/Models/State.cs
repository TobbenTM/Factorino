using System;
using System.Collections.Generic;

namespace FNO.Broker.Models
{
    public class State
    {
        public State()
        {
            Players = new Dictionary<Guid, BrokerPlayer>();
            Orders = new Dictionary<Guid, BrokerOrder>();
            Shipments = new Dictionary<Guid, BrokerShipment>();
            FactoryOwners = new Dictionary<Guid, BrokerPlayer>();
            HandledTransactions = new Queue<Guid>();
            HandledShipments = new Queue<Guid>();
        }

        /// <summary>
        /// Players indexed by player id
        /// </summary>
        public Dictionary<Guid, BrokerPlayer> Players { get; }

        /// <summary>
        /// Orders indexed by order id
        /// </summary>
        public Dictionary<Guid, BrokerOrder> Orders { get; }

        /// <summary>
        /// Shipments indexed by shipment id
        /// </summary>
        public Dictionary<Guid, BrokerShipment> Shipments { get; }

        /// <summary>
        /// Factory owners indexed by factory id
        /// </summary>
        public Dictionary<Guid, BrokerPlayer> FactoryOwners { get; }

        /// <summary>
        /// We need track of transactions already handled
        /// </summary>
        public Queue<Guid> HandledTransactions { get; }

        /// <summary>
        /// We need track of shipments already handled
        /// </summary>
        public Queue<Guid> HandledShipments { get; }
    }
}
