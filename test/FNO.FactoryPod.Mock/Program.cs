﻿using FNO.EventSourcing;

namespace FNO.FactoryPod.Mock
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
