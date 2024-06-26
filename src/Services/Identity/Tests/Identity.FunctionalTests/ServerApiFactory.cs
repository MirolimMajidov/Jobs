using Microsoft.AspNetCore.Mvc.Testing;

namespace Identity.FunctionalTests
{
    using IdentityService;
    using IdentityService.DataProvider;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    public class ServerApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                //// Remove the existing context configuration
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<JobsContext>));
                //if (descriptor != null)
                //    services.Remove(descriptor);

                //// Add an in-memory database for testing
                //services.AddDbContext<JobsContext>(options =>
                //{
                //    options.UseInMemoryDatabase("JobDB");
                //});
            });
        }
    }
}
