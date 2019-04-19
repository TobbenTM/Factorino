using System;
using FNO.Domain.Models;

namespace FNO.Domain.Events.Player
{
    public class PlayerInventoryChangedEvent : EntityEvent
    {
        public LuaItemStack[] InventoryChange { get; set; }

        public PlayerInventoryChangedEvent()
        {
        }

        public PlayerInventoryChangedEvent(Guid entityId, Models.Player initiator) : base(entityId, initiator)
        {
        }
    }
}
