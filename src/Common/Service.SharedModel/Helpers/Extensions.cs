using Jobs.SharedModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Service.SharedModel.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        ///     A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            var customAttribute = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();
            return customAttribute?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Cast string to enum type
        /// </summary>
        public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
                return defaultValue;

            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }

        /// <summary>
        /// Changing enum and other object value's type
        /// </summary>
        public static object ConvertTo(this object value, Type typeTo)
        {
            try
            {
                if (value == null || value is DBNull) return null;

                if (typeTo.IsEnum)
                {
                    return Enum.ToObject(typeTo, value);
                }
                else if (typeTo.IsArray)
                {
                    Type type = typeTo.GetElementType();
                    var imputValue = value.ToString().TrimStart('[').TrimEnd(']');
                    if (type.Name == "String")
                    {
                        var elements = Regex.Replace(imputValue, @"\t|\n|\r| |""", string.Empty).Split(',');
                        return elements.Select(s => (string)Convert.ChangeType(s, type)).ToArray();
                    }
                    else
                    {
                        var elements = imputValue.Split(',');
                        return elements.Select(s => (int)Convert.ChangeType(s, type)).ToArray();
                    }
                }
                else if (typeTo.Name == nameof(Guid))
                {
                    return Guid.Parse(value.ToString());
                }
                else
                {
                    if (typeTo.IsGenericType && typeTo.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return Convert.ChangeType(value, Nullable.GetUnderlyingType(typeTo));
                    else
                        return Convert.ChangeType(value, typeTo);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"The server couldn't cast '{value}' object to '{typeTo.Name}' type. Error: {ex.Message}");
            }
        }

        /// <summary>   
        /// This is for getting entities by type from data base.
        /// </summary>   
        public static IQueryable<T> GetEntities<T>(this DbContext context) where T : BaseEntity => context.Set<T>();
    }
}
