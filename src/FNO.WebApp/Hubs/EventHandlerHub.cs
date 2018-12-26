using Microsoft.AspNetCore.SignalR;

namespace FNO.WebApp.Hubs
{
    public abstract class EventHandlerHub : Hub<IEventHandlerClient>
    {
    }
}
