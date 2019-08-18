using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Will subscribe a client to all events related to the entites
        /// </summary>
        /// <param name="entities">The entities to subscribe to events for</param>
        protected async Task Subscribe(IEnumerable<Guid> entities)
        {
            var tasks = new List<Task>();
            foreach (var entity in entities)
            {
                tasks.Add(Subscribe(entity));
            }
            await Task.WhenAll(tasks.ToArray());
        }

        /// <summary>
        /// Will unsubscribe a client to all events related to a entity
        /// </summary>
        /// <param name="entityId">The entity to subscribe to events for</param>
        protected async Task UnSubscribe(Guid entityId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, entityId.ToString());
        }

        /// <summary>
        /// Will unsubscribe from all groups
        /// </summary>
        protected Task UnSubscribeFromAll()
        {
            // TODO throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
