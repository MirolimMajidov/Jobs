using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.RabbitMQ;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentService.DataProvider;
using PaymentService.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PaymentService
{
    public class Program
    {
        public const string AppName = "Payment service";

        public static async Task Main(string[] args)
        {
            try
            {
                ConfigureLogging();
                Log.Information("Starting web host ({ApplicationContext})...", AppName);

                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var dbContext = services.GetRequiredService<IJobsMongoContext>() as JobsMongoContext;
                var transactionRepository = services.GetRequiredService<IEntityRepository<Transaction>>();
                await dbContext.SeedAsync(logger, transactionRepository);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.ConfigureContainer<ContainerBuilder>((container) =>
            {
                //container.Populate(services);
                container.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRabbitMQEventHandler<>));
            });

            builder.ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddSerilog();
                });
            builder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return builder;
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/Log.txt",
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
        }
    }
}
