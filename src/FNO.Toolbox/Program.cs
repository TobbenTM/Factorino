using FNO.Toolbox.ProduceEvent;
using FNO.Toolbox.StarterKit;
using Terminal.Gui;

namespace FNO.Toolbox
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var win = new Window("Factorino Toolbox")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            var welcome = new Label("Welcome to the Factorino toolbox")
            {
                X = Pos.Center(),
                Y = Pos.Center() - 2,
                Height = 1,
            };
            win.Add(welcome);

            var produceBtn = new Button("Produce events")
            {
                X = Pos.Center(),
                Y = Pos.Center() + 1,
                Height = 1,
            };
            produceBtn.Clicked += delegate ()
            {
                var producer = new SelectEventDialog();
                Application.Run(producer);
            };
            win.Add(produceBtn);

            var startKitBtn = new Button("Give Starter Kit")
            {
                X = Pos.Center(),
                Y = Pos.Center() + 3,
                Height = 1,
            };
            startKitBtn.Clicked += delegate ()
            {
                var starterKitDialog = new StarterKitDialog();
                Application.Run(starterKitDialog);
            };
            win.Add(startKitBtn);

            var quitBtn = new Button("Quit")
            {
                X = Pos.Center(),
                Y = Pos.Center() + 5,
                Height = 1,
            };
            quitBtn.Clicked += delegate ()
            {
                top.Running = false;
            };
            win.Add(quitBtn);

            top.Add(win);
            Application.Run();
        }
    }
}
