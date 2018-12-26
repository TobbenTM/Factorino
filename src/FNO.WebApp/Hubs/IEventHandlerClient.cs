using System.Threading.Tasks;
using FNO.Domain.Events;

namespace FNO.WebApp.Hubs
{
    public interface IEventHandlerClient
    {
        Task ReceiveEvent(IEvent evnt);
    }
}
