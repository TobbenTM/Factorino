using FNO.Common;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading;

namespace FNO.ReadModel
{
    internal class Program : IDisposable
    {
        static void Main(string[] args)
        {
            using (var program = new Program())
            {
                program.Run();
            }
        }

        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly Daemon _daemon;

        public Program()
        {
            _configuration = Configuration.GetConfiguration();
            _logger = Logging.GetLogger(_configuration);

            // Handle user exit (CTRL + C) gracefully
            Console.CancelKeyPress += new ConsoleCancelEventHandler((_, e) =>
            {
                // Prevent premature app termination
                e.Cancel = true;
                // Allow graceful exit
                _resetEvent.Set();
            });

            // Handle system exit gracefully
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((_, e) => _resetEvent.Set());

            _daemon = new Daemon(_configuration, _logger);
        }

        public void Run()
        {
            _daemon.Run();

            _resetEvent.WaitOne();
        }

        public void Dispose()
        {
            _daemon.Dispose();
        }
    }
}
