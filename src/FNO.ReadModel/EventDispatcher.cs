using System;
using System.Threading.Tasks;
using FNO.Domain.Events;

namespace FNO.ReadModel
{
    class EventDispatcher : IEventDispatcher
    {
        public Task Handle(IEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}
