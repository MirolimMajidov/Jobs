using Microsoft.AspNetCore.Mvc.Testing;

namespace Identity.IntegrationTests
{
    using IdentityService;
    using IdentityService.DataProvider;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

    public class ServerApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Remove the existing context configuration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<JobsContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add an in-memory database for testing
                services.AddDbContext<JobsContext>(options =>
                {
                    options.UseInMemoryDatabase("JobDB");
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbContext = services.GetRequiredService<JobsContext>();

                dbContext.SeedAsync(logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");

                throw;
            }

            return host;
        }
    }
}
