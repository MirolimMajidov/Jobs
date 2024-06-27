using Autofac.Extensions.DependencyInjection;
using JobService.DataProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JobService
{
    public class Program
    {
        public const string AppName = "Job service";

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbContext = services.GetRequiredService<JobsContext>();
                if (dbContext.Database.IsSqlServer())
                    dbContext.Database.Migrate();

                await dbContext.SeedAsync(logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddNLog();
            });
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel((context, options) =>
                {
                    var httpPort = context.Configuration.GetValue("HTTP_PORT", 8080);
                    options.Listen(IPAddress.Any, httpPort,
                        listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
                });
                webBuilder.UseStartup<Startup>();
            });

            return builder;
        }
    }
}
