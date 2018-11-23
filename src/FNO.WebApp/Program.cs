using FNO.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace FNO.WebApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = Logging.GetLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder().Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
