using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus.RabbitMQ;
using FluentValidation.AspNetCore;
using Jobs.Service.Common.Configurations;
using Jobs.Service.Common.Infrastructure.Exceptions;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseConfiguration>(Configuration.GetSection("DatabaseConfiguration"));
            services.AddScoped<JobsContext>();
            services.UseEventBusRabbitMQ(Configuration["RabbitMQHostName"], Configuration["SubscriptionClientName"], int.Parse(Configuration["EventBusRetryCount"]));

            services.AddAuthenticationsAndPolices();
            services.AddControllers(options => options.Filters.Add(typeof(JobsExceptionFilter)))
                .AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddResponseNewtonsoftJson();
            services.AddJobsHealthChecks();
            services.AddSwaggerGen("Payment");
            services.AddAutoMapper(typeof(DTOMapper));

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRabbitMQEventHandler<>));
            return new AutofacServiceProvider(container.Build());
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
