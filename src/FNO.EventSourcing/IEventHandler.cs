using FNO.Domain.Events;
using System.Threading.Tasks;

namespace FNO.EventSourcing
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent evnt);
    }
}
