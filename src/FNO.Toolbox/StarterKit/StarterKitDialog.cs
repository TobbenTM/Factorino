using System;
using System.Linq;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Toolbox.Common;
using Terminal.Gui;

namespace FNO.Toolbox.StarterKit
{
    class StarterKitDialog : Dialog
    {
        public StarterKitDialog() : base("Starter Kit", 0, 0)
        {
            Width = 52;
            Height = 10;

            var label = new Label("PlayerId")
            {
                X = 2,
                Y = 2,
                Width = 20,
                Height = 1,
            };
            Add(label);

            var field = new TextField("")
            {
                Id = "playerId",
                X = 22,
                Y = Pos.Top(label),
                Width = 22,
                Height = 1,
            };
            Add(field);

            var btn = new Button("Give")
            {
                X = Pos.Center(),
                Y = 4,
                Clicked = delegate ()
                {
                    var rng = new Random();
                    var playerId = Guid.Parse(field.Text.ToString());

                    var evnts = Domain.Seed.EntityLibrary.Data()
                        .Select(e => new PlayerInventoryChangedEvent(playerId, new Player { Name = "<toolbox>" })
                        {
                            InventoryChange = new[]
                            {
                                new LuaItemStack
                                {
                                    Name = e.Name,
                                    Count = rng.Next(10, 1000000),
                                },
                            },
                        })
                        .ToArray();


                    var producer = new ProducerDialog();
                    producer.Produce(evnts);
                    Application.Run(producer);
                },
            };
            Add(btn);
        }
    }
}
