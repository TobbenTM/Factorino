using FNO.Domain.Events;
using System.Threading.Tasks;

namespace FNO.EventSourcing
{
    public interface IEventDispatcher
    {
        Task Handle<TEvent>(TEvent evnt) where TEvent : IEvent;
    }
}
