using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FNO.WebApp.Hubs
{
    public abstract class EventHandlerHub : Hub<IEventHandlerClient>
    {
        /// <summary>
        /// Will subscribe a client to all events related to a entity
        /// </summary>
        /// <param name="entityId">The entity to subscribe to events for</param>
        protected async Task Subscribe(Guid entityId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, entityId.ToString());
        }
    }
}
