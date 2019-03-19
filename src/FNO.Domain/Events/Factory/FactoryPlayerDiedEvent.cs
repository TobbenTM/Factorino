using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryPlayerDiedEvent : FactoryActivityBaseEvent
    {
        public FactoryPlayerDiedEvent()
        {
        }

        public FactoryPlayerDiedEvent(Guid factoryId, string type, long tick) : base(factoryId, type, tick)
        {
        }

        public string PlayerName { get; set; }

        public override string ReadableEvent => $"{PlayerName} died a horrible death";
    }
}
