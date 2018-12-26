using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FNO.WebApp.Hubs
{
    [Authorize]
    public class FactoryCreateHub : EventHandlerHub
    {

    }
}
