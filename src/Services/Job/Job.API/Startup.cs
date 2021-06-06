using Identity.API.Infrastructure.HealthChecks;
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
using Service.SharedModel.Exceptions;
using Service.SharedModel.Repository;
using System;

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
            services.AddDbContext<JobsContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DBConnection"), sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));
            services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobExceptionFilter))).AddResponseNewtonsoftJson();
            services.AddJobsHealthChecks().AddCheck("SQL Server", new SqlServerHealthCheck(Configuration.GetConnectionString("DBConnection")));
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
                endpoints.MapJobsHealthChecks();
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Welcome to 'Job' service!");
            });
        }
    }
}
