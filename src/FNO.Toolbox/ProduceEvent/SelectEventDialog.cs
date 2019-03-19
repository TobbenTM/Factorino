using FNO.Domain.Events;
using System;
using System.Linq;
using System.Reflection;
using Terminal.Gui;

namespace FNO.Toolbox.ProduceEvent
{
    internal class SelectEventDialog : Dialog
    {
        private readonly Type[] _events;
        private readonly ListView _eventOptions;

        private int _selectedIndex;

        private Type SelectedEvent => _events[_selectedIndex];

        public SelectEventDialog() : base("Choose event", 0, 0)
        {
            Width = 50;
            Height = 20;
            _events = Assembly.GetAssembly(typeof(IEvent)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Event)))
                .ToArray();

            _eventOptions = new ListView(_events.Select(t => $" - {t.Name}").ToList());

            var container = new ScrollView(new Rect(0, 0, 46, 16))
            {
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                ContentSize = new Size(44, _events.Length),
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = false,
            };

            _eventOptions.SelectedChanged += delegate ()
            {
                if (_selectedIndex < _eventOptions.SelectedItem && _eventOptions.SelectedItem > 5)
                {
                    container.ScrollDown(1);
                }
                else if (_selectedIndex > _eventOptions.SelectedItem && _eventOptions.SelectedItem < _events.Length - 6)
                {
                    container.ScrollUp(1);
                }
                _selectedIndex = _eventOptions.SelectedItem;
            };

            container.Add(_eventOptions);
            Add(container);
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (kb.Key == Key.Enter)
            {
                Clear();
                var creator = new CreateEventDialog(SelectedEvent);
                Application.Run(creator);
                return true;
            }
            return base.ProcessKey(kb);
        }
    }
}
