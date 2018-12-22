using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FNO.Common
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            // TODO: Add global appsettings

            // Add local appsettings
            builder.AddJsonFile("appsettings.json");

            // Add local user appsettings
            builder.AddJsonFile("appsettings.user.json", optional: true);

            // If we're running on windows, try to add any windows specific settings
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                builder.AddJsonFile("appsettings.windows.json", optional: true);
            }

            // Add environment variables
            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        public static TConfiguration Bind<TConfiguration>(this IConfiguration configuration) 
            where TConfiguration : ConfigurationBase, new()
        {
            var config = new TConfiguration();
            configuration.Bind(config);
            return config;
        }
    }
}
