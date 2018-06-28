using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryOutgoingTrainEvent : FactoryActivityBaseEvent
    {
        public FactoryOutgoingTrainEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string TrainName { get; set; }
        public LuaItemStack[] Inventory { get; set; }
    }
}
