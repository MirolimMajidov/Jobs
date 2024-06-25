using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.RabbitMQ;
using FluentValidation;
using FluentValidation.AspNetCore;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentService.Configurations;
using PaymentService.DataProvider;
using PaymentService.Mapping;
using PaymentService.RabbitMQEvents.EventHandlers;
using PaymentService.RabbitMQEvents.Events;
using PaymentService.Repository;
using System;
using System.Reflection;

namespace PaymentService
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
            services.Configure<DatabaseConfiguration>(options =>
            {
                var databaseSection = Configuration.GetSection(nameof(DatabaseConfiguration));
                var databaseInfo = databaseSection.Get<DatabaseConfiguration>();

                options.DatabaseName = databaseInfo.DatabaseName;
                options.PaymentsName = databaseInfo.PaymentsName;
                options.ConnectionString = Configuration["ConnectionString"];
            });

            services.AddStackExchangeRedisCache(options => options.Configuration = Configuration["RedisConnectionString"]);

            services.AddScoped<IJobsMongoContext, JobsMongoContext>();
            services.AddTransient(typeof(IEntityRepository<>), typeof(RedisEntityRepository<>));
            services.UseEventBusRabbitMQ(Configuration["RabbitMQConnection"], Configuration["SubscriptionClientName"], int.Parse(Configuration["EventBusRetryCount"]));

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobsExceptionFilter)))
                .AddResponseNewtonsoftJson();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddJobsHealthChecks();
            services.AddSwaggerGen("Payment");
            services.AddAutoMapper(typeof(DTOMapper));
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
                await context.Response.WriteAsync("Welcome to 'Payment' service!");
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
