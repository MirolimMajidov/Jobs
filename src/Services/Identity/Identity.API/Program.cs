using IdentityService.DataProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService
{
    public class Program
    {
        public const string AppName = "Identity service";

        public static async Task Main(string[] args)
        {
            var configuration = GetConfiguration();
            var host = CreateHostBuilder(configuration, args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbContext = services.GetRequiredService<JobsContext>();
                if (dbContext.Database.IsMySql())
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

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                    logging.AddNLog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(options =>
                    {
                        var (httpPort, grpcPort) = GetDefinedPorts(configuration);
                        options.Listen(IPAddress.Any, httpPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
                        options.Listen(IPAddress.Any, grpcPort,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                    })
                    .UseStartup<Startup>();
                });

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration configuration)
        {
            var grpcPort = configuration.GetValue("GRPC_PORT", 81);
            var port = configuration.GetValue("PORT", 80);
            return (port, grpcPort);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var fileName = "appsettings.json";
#if DEBUG
            fileName = "appsettings.Development.json";
#endif   
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName, false, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
