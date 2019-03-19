using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerMinedItemEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerMinedItemEvent()
        {
        }

        public FactoryPlayerMinedItemEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }
        public LuaItemStack ItemStack { get; set; }

        public override string ReadableEvent => $"{PlayerName} picked up {ItemStack.Count} {ItemStack.Name}(s)";
    }
}
