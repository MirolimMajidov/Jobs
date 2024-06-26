using Autofac;
using EventBus.RabbitMQ;
using FluentValidation;
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
                var databaseInfo = Configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();
                options.DatabaseName = databaseInfo.DatabaseName;
                options.ConnectionString = databaseInfo.ConnectionString;
            });

            services.AddStackExchangeRedisCache(options => options.Configuration = Configuration["RedisConnectionString"]);

            services.AddScoped<IJobsMongoContext, JobsMongoContext>();
            services.AddTransient(typeof(IEntityRepository<>), typeof(RedisEntityRepository<>));

            var rabbitMQConfigInfo = Configuration.GetSection(nameof(RabbitMQConfigurationInfo)).Get<RabbitMQConfigurationInfo>();
            services.UseEventBusRabbitMQ(rabbitMQConfigInfo);

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobsExceptionFilter)))
                .AddResponseNewtonsoftJson();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddJobsHealthChecks();
            services.AddSwaggerGen("Payment");
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
