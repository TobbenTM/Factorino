using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FNO.WebApp.Security
{
    public static class AuthenticationConfiguration
    {
        public const string SteamCookieScheme = SteamAuthenticationDefaults.AuthenticationScheme + CookieAuthenticationDefaults.AuthenticationScheme;

        public static IServiceCollection AddFactorinoAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var steamAppKey = config["Authentication:Steam:AppKey"];
            if (string.IsNullOrEmpty(steamAppKey))
            {
                throw new ArgumentException($"Missing Steam App Key!");
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cfg =>
                {
                    cfg.LoginPath = "/auth/login";
                    cfg.AccessDeniedPath = "/auth/accessdenied";
                })
                .AddCookie(SteamCookieScheme)
                .AddSteam(SteamAuthenticationDefaults.AuthenticationScheme, cfg =>
                {
                    cfg.SignInScheme = SteamCookieScheme;
                    cfg.ApplicationKey = steamAppKey;
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
