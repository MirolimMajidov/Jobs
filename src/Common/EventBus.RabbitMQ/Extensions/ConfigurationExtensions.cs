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
        /// To register all needed structure of RabbitMQ
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configurationInfo">Configuration info to configure the RabbitMQ</param>
        public static void UseEventBusRabbitMQ(this IServiceCollection services, RabbitMQConfigurationInfo configurationInfo)
        {
            services.AddSingleton<IRabbitMQConnection>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQConnection>>();
                var factory = new ConnectionFactory { HostName = configurationInfo.ConnectionString, DispatchConsumersAsync = true };

                return new RabbitMQConnection(factory, logger, configurationInfo.RetryPublishCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();

            services.AddSingleton<IEventBusRabbitMQ, EventBusRabbitMQ>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var rabbitMqConnection = serviceProvider.GetRequiredService<IRabbitMQConnection>();
                var lifetimeScope = serviceProvider.GetRequiredService<ILifetimeScope>();
                var subscriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMqConnection, subscriptionsManager, lifetimeScope, logger,
                    configurationInfo.SubscriptionName);
            });
        }
    }
}
