using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Linq;

namespace EventBus.RabbitMQ
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// To register all needed sturucture of RabbitMQ
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="hostName">Host name of RabbitMQ connection</param>
        /// <param name="clientName">Subscription's client name</param>
        /// <param name="retryCount">Retry count</param>
        public static void UseEventBusRabbitMQ(this IServiceCollection services, string hostName = "localhost", string clientName = "localhost", int retryCount = 5)
        {
            services.AddSingleton<IRabbitMQConnection>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQConnection>>();
                var factory = new ConnectionFactory { HostName = hostName, DispatchConsumersAsync = true };

                return new RabbitMQConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();

            services.AddSingleton<IEventBusRabbitMQ, EventBusRabbitMQ>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMqConnection = serviceProvider.GetRequiredService<IRabbitMQConnection>();
                var iLifetimeScope = serviceProvider.GetRequiredService<ILifetimeScope>();
                var subscriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMqConnection, subscriptionsManager, iLifetimeScope, logger,
                    clientName);
            });
        }
    }
}
