using FNO.Domain.Models;
using System;
using System.Linq;

namespace FNO.Domain.Events.Factory
{
    public class FactoryOutgoingTrainEvent : FactoryActivityBaseEvent
    {
        public FactoryOutgoingTrainEvent()
        {
        }

        public FactoryOutgoingTrainEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string TrainName { get; set; }
        public LuaItemStack[] Inventory { get; set; }

        public override string ReadableEvent => $"A train left the factory with {Inventory?.Sum(stack => stack.Count) ?? 0} items";
    }
}
