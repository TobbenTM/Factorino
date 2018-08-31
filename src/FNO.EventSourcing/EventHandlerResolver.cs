using FNO.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNO.EventSourcing
{
    public class EventHandlerResolver
    {
        private readonly IDictionary<Type, List<Func<IHandler>>> _handlers;

        public EventHandlerResolver()
        {
            _handlers = new Dictionary<Type, List<Func<IHandler>>>();
        }

        public void Register(Func<IHandler> handler, params Type[] types)
        {
            foreach (var type in types)
            {
                if (!_handlers.ContainsKey(type))
                {
                    _handlers[type] = new List<Func<IHandler>>();
                }
                _handlers[type].Add(handler);
            }
        }

        public IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var type = evnt.GetType();
            if (!_handlers.ContainsKey(type)) yield break;
            var handlers = _handlers[type].Select(h => h());
            foreach (var handler in handlers)
            {
                var eventHandler = handler as IEventHandler<TEvent>;
                if (eventHandler != null)
                {
                    yield return eventHandler;
                }
            }
        }
    }
}
