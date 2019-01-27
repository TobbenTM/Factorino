using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace FNO.EventSourcing
{
    public interface IConsumerDaemon : IDisposable
    {
        void Init(IConfiguration configuration, ILogger logger);
        void Run();
    }
}
