using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FNO.WebApp.Security
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddFactorinoAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var steamAppKey = config["Authentication:Steam:AppKey"];
            if (string.IsNullOrEmpty(steamAppKey))
            {
                throw new ArgumentException($"Missing Steam App Key!");
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddSteam(cfg =>
                {
                    cfg.SignInScheme = "steam";
                    cfg.ApplicationKey = steamAppKey;
                })
                .AddCookie("steam")
                .AddCookie(cfg =>
                {
                    cfg.LoginPath = "/auth/login";
                });

            return services;
        }

        public static IApplicationBuilder UseFactorinoAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            return app;
        }
    }
}
