using Terminal.Gui;

namespace FNO.Toolbox
{
    internal class Program
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

            var welcome = new Label("Welcome to the Factorino toolbox!")
            {
                X = Pos.Center(),
                Y = Pos.Center() - 2,
                Width = 35,
                Height = 1,
            };
            win.Add(welcome);

            var produceBtn = new Button("_Produce event")
            {
                X = Pos.Center(),
                Y = Pos.Center() + 1,
                Width = 15,
                Height = 1,
            };
            produceBtn.Clicked += delegate () {
                var producer = new EventProducer();
                Application.Run(producer);
            };
            win.Add(produceBtn);

            var quitBtn = new Button("_Quit")
            {
                X = Pos.Center(),
                Y = Pos.Center() + 3,
                Width = 5,
                Height = 1,
            };
            quitBtn.Clicked += delegate () {
                top.Running = false;
            };
            win.Add(quitBtn);

            top.Add(win);
            Application.Run();
        }
    }
}
