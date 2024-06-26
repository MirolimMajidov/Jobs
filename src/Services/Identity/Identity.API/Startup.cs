using Autofac;
using EventBus.RabbitMQ;
using FluentValidation;
using Identity.API.Infrastructure.HealthChecks;
using IdentityService.DataProvider;
using IdentityService.RabbitMQEvents.EventHandlers;
using IdentityService.RabbitMQEvents.Events;
using IdentityService.Repository;
using IdentityService.Services;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace IdentityService
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddGrpc();

            services.AddDbContext<JobsContext>(o =>
            o.UseMySql(Configuration["ConnectionString"], new MySqlServerVersion(new Version(8, 0, 21)),
            sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddTransient(typeof(IEntityQueryableRepository<>), typeof(EntityRepository<>));

            int eventBusRetryCount = Configuration.GetSection("Environment").Value == "Test" ? 0 : int.Parse(Configuration["EventBusRetryCount"]);
            services.UseEventBusRabbitMQ(Configuration["RabbitMQConnection"], Configuration["SubscriptionClientName"], eventBusRetryCount);

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobsExceptionFilter)))
                .AddResponseNewtonsoftJson();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddJobsHealthChecks().AddCheck("MySQL", new MySqlHealthCheck(Configuration["ConnectionString"]));
            services.AddSwaggerGen("Identity");
            services.AddAutoMapper(typeof(Program));
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.AddRabbitMQEventHandlers(typeof(Startup));
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

            app.UseEventBus();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapJobsHealthChecks();
                endpoints.MapGrpcService<UserService>();
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Welcome to 'Identity' service!");
            });
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static void UseEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusRabbitMQ>();
            eventBus.Subscribe<UserPaymentEvent, UserPaymentEventHandler>();
        }
    }
}
