using Autofac;
using EventBus.RabbitMQ;
using FluentValidation;
using Identity.API.Infrastructure.HealthChecks;
using Jobs.Service.Common;
using JobService.DataProvider;
using JobService.Mapping;
using JobService.RabbitMQEvents.EventHandlers;
using JobService.RabbitMQEvents.Events;
using JobService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<JobsContext>(o => o.UseSqlServer(Configuration["ConnectionString"], sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

            services.AddTransient(typeof(IEntityQueryableRepository<>), typeof(EntityRepository<>));

            var rabbitMQConfigInfo = Configuration.GetSection(nameof(RabbitMQConfigurationInfo)).Get<RabbitMQConfigurationInfo>();
            services.UseEventBusRabbitMQ(rabbitMQConfigInfo);

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobsExceptionFilter)))
                .AddResponseNewtonsoftJson();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddJobsHealthChecks().AddCheck("SQL Server", new SqlServerHealthCheck(Configuration["ConnectionString"]));
            services.AddSwaggerGen("Job");
            services.AddAutoMapper(typeof(DTOMapper));
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.AddRabbitMQEventHandlers(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerDocs();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEventBus();

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

    public static class ApplicationBuilderExtensions
    {
        public static void UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusRabbitMQ>();
            eventBus.Subscribe<UserNameUpdatedEvent, UserNameUpdatedEventHandler>();
        }
    }
}
