using System.Linq;
using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;
using FNO.EventSourcing;
using Serilog;

namespace FNO.ReadModel.EventHandlers
{
    public class ShippingEventHandler : EventHandlerBase,
        IEventHandler<ShipmentRequestedEvent>,
        IEventHandler<ShipmentFulfilledEvent>,
        IEventHandler<ShipmentReceivedEvent>,
        IEventHandler<ShipmentCompletedEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public ShippingEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(ShipmentRequestedEvent evnt)
        {
            _dbContext.Shipments.Add(new Shipment
            {
                ShipmentId = evnt.EntityId,
                OwnerId = evnt.OwnerId,
                FactoryId = evnt.FactoryId,
                DestinationStation = evnt.DestinationStation,
                Carts = evnt.Carts,
                WaitConditions = evnt.WaitConditions,
                State = ShipmentState.Requested,
            });
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentFulfilledEvent evnt)
        {
            var shipment = _dbContext.Shipments.FirstOrDefault(s => s.ShipmentId == evnt.EntityId);
            if (shipment != null)
            {
                shipment.State = ShipmentState.Fulfilled;
            }
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentReceivedEvent evnt)
        {
            var shipment = _dbContext.Shipments.FirstOrDefault(s => s.ShipmentId == evnt.EntityId);
            if (shipment != null)
            {
                shipment.State = ShipmentState.Received;
            }
            return Task.CompletedTask;
        }

        public Task Handle(ShipmentCompletedEvent evnt)
        {
            var shipment = _dbContext.Shipments.FirstOrDefault(s => s.ShipmentId == evnt.EntityId);
            if (shipment != null)
            {
                shipment.State = ShipmentState.Completed;
            }
            return Task.CompletedTask;
        }
    }
}
