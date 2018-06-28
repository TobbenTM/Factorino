using Microsoft.Extensions.Configuration;
using Serilog;

namespace FNO.Common
{
    public static class Logging
    {
        public static ILogger GetLogger(IConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = Configuration.GetConfiguration();
            }

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
