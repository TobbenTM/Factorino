using System;

namespace FNO.Domain.Events.Factory
{
    public class FactoryOnlineEvent : EntityEvent
    {
        public FactoryOnlineEvent(Guid factoryId) : base(factoryId, null)
        {
        }

        public override string ReadableEvent => "The factory came online";
    }
}
