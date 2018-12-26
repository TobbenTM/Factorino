using FNO.WebApp.Hubs;
using Microsoft.AspNetCore.Builder;

namespace FNO.WebApp.Services
{
    public static class SignalR
    {
        public static IApplicationBuilder UseFactorinoSignalR(this IApplicationBuilder app)
        {
            app.UseSignalR(conf =>
            {
                conf.MapHub<WorldHub>("/world");
                conf.MapHub<ActivityHub>("/activity");
                conf.MapHub<FactoryCreateHub>("/factorycreate");
            });

            return app;
        }
    }
}
