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
                conf.MapHub<WorldHub>("/ws/world");
                conf.MapHub<FactoryHub>("/ws/factory");
                conf.MapHub<MarketHub>("/ws/market");
                conf.MapHub<ShippingHub>("/ws/shipping");
                conf.MapHub<PlayerHub>("/ws/player");
                conf.MapHub<FactoryCreateHub>("/ws/factorycreate");
            });

            return app;
        }
    }
}
