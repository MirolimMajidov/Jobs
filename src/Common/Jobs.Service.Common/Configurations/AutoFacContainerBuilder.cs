using Autofac;
using EventBus.RabbitMQ;
using System;
using System.Reflection;

namespace Jobs.Service.Common
{
    public static class AutoFacContainerBuilderExtensions
    {
        /// <summary>
        /// To find and register all a event handler classes for receiving event from the RabbitMQ.
        /// </summary>
        /// <param name="type">Any type to get assembly and find event handler classes from that</param>
        public static void AddRabbitMQEventHandlers(this ContainerBuilder container, Type type)
        {
            container.RegisterAssemblyTypes(type.GetTypeInfo().Assembly)
                     .AsClosedTypesOf(typeof(IRabbitMQEventHandler<>));
        }
    }
}
