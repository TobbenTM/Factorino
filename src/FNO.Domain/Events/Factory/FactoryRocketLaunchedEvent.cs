using FNO.Domain.Models;
using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryRocketLaunchedEvent : FactoryActivityBaseEvent
    {
        public FactoryRocketLaunchedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public LuaEntity Rocket { get; set; }
    }
}
