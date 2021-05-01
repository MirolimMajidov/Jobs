using JobService.DataProvider;
using JobService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.SharedModel.Configurations;
using Service.SharedModel.Repository;

namespace JobService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;
#if DEBUG
            connectionString = Configuration.GetConnectionString("LocalSQLConnection");
#else
            connectionString = Configuration.GetConnectionString("SQLConnection");
#endif
            services.AddDbContext<JobsContext>(o => o.UseSqlServer(connectionString));
            services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));

            services.AddAuthentications();
            services.AddControllers().AddResponseJsonOptions();
            services.AddSwaggerGen("Job");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerDocs();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Welcome to 'Job' service!");
            });
        }
    }
}
