﻿using FNO.EventSourcing;

namespace FNO.ReadModel
{
    internal class Program : ConsumerBase<Daemon>
    {
        private static void Main(string[] args)
        {
            using (var program = new Program())
            {
                program.Run();
            }
        }
    }
}
