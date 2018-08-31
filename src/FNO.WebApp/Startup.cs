using FNO.Common;
using FNO.Domain;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.EventStream;
using FNO.WebApp.Security;
using FNO.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace FNO.WebApp
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = Configuration.GetConfiguration();
            _logger = Logging.GetLogger(_configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration.Bind<ConfigurationBase>());
            services.AddSingleton(_configuration);
            services.AddSingleton(_logger);

            services.AddMvc();
            services.AddSignalR();
            services.AddFactorinoAuthentication(_configuration);
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new Info { Title = "Factorino API", Version = "v1" });
            });

            services.AddDbContext<ReadModelDbContext>(opts =>
            {
                ReadModelDbContext.ConfigureBuilder(opts, _configuration);
            });

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IFactoryRepository, FactoryRepository>();
            services.AddScoped<ICorporationRepository, CorporationRepository>();
            services.AddScoped<IEventStore, KafkaProducer>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                RequireHeaderSymmetry = false,
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseFactorinoAuthentication();

            app.UseFactorinoSignalR();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Factorino API v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
