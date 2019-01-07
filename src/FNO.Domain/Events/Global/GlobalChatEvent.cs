using System;

namespace FNO.Domain.Events.Global
{
    public class GlobalChatEvent : EntityEvent
    {
        public GlobalChatEvent(Guid originId) : base(originId, null)
        {
        }

        public string Message { get; set; }

        public Models.Player Player { get; set; }
    }
}
