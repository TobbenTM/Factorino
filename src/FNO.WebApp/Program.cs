using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FNO.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder().Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
    }
}
