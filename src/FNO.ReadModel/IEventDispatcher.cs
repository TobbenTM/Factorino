using FNO.Domain.Events;
using System.Threading.Tasks;

namespace FNO.ReadModel
{
    public interface IEventDispatcher
    {
        Task Handle(IEvent evnt);
    }
}
