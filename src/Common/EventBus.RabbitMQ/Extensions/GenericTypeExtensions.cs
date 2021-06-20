﻿using System;
using System.Linq;

namespace EventBus.RabbitMQ
{
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// To get name of generic type
        /// </summary>
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }
    }
}
