using FNO.Domain.Events;
using System.Threading.Tasks;

namespace FNO.EventStream
{
    public interface IEventConsumer
    {
        Task HandleEvent<TEvent>(TEvent evnt) where TEvent : IEvent;
        void OnEndReached(string topic, int partition, long offset);
    }
}
