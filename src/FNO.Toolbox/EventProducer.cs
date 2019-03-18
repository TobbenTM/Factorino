using FNO.Domain.Events;
using System;
using System.Linq;
using System.Reflection;
using Terminal.Gui;

namespace FNO.Toolbox
{
    internal class EventProducer : Dialog
    {
        private readonly Type[] _events;
        private readonly ListView _eventOptions;

        private EventProducerState _state = EventProducerState.Choosing;
        private Type _selectedEvent;

        public EventProducer() : base("Choose event", 0, 0)
        {
            Width = 50;
            Height = 20;
            _events = Assembly.GetAssembly(typeof(IEvent)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Event)))
                .ToArray();

            //_eventOptions = new ListView(_events.Select(t => $" - {t.Name}").ToList());

            var container = new ScrollView(new Rect(0, 0, 46, 16))
            {
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                ContentSize = new Size(44, _events.Length),
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = false,
            };

            //container.Add(_eventOptions);
            container.Add(_events.Select((type, index) => new Button($" - {type.Name}")
            {
                Clicked = () => CreateEvent(type),
                Y = index,
            }).ToArray());
            Add(container);
        }

        private void CreateEvent(Type eventType)
        {
            throw new NotImplementedException();
        }
    }

    internal enum EventProducerState
    {
        Choosing = 0,
        Enriching = 1,
        Producing = 2,
    }
}
