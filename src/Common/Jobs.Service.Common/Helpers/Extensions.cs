using Jobs.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Jobs.Service.Common.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// This is for get user Id from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>User Id</returns>
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var result = Guid.TryParse(user.Claims.SingleOrDefault(c => c.Type == "UserId")?.Value, out Guid userId);
            if (result)
                return userId;
            else
                return Guid.Empty;
        }

        /// <summary>
        /// This is for get user name from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>User name</returns>
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// This is for get user role from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>User role</returns>
        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// This is for get user role Id from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>User role Id</returns>
        public static int GetUserRoleId(this ClaimsPrincipal user)
        {
            var result = int.TryParse(user.Claims.SingleOrDefault(c => c.Type == "RoleId")?.Value, out int statusId);
            if (result)
                return statusId;
            else
                return 0;
        }

        /// <summary>
        /// This is for check admin from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>True or false</returns>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(UserRole.Admin.GetDisplayName());
        }

        /// <summary>
        /// This is for check SuperAdmin from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>True or false</returns>
        public static bool IsSuperAdmin(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(UserRole.SuperAdmin.GetDisplayName());
        }

        /// <summary>
        /// This is for check SuperAdmin from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>True or false</returns>
        public static bool IsEditor(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(UserRole.Editor.GetDisplayName());
        }

        /// <summary>
        /// This is for check User from User of Context
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>True or false</returns>
        public static bool IsUser(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated && user.IsInRole(UserRole.User.GetDisplayName());
        }

        /// <summary>
        /// It will be true when user is SuperAdmin or Admin
        /// </summary>
        /// <param name="user">User of Context</param>
        /// <returns>True or false</returns>
        public static bool IsAdministrators(this ClaimsPrincipal user)
        {
            return user.IsSuperAdmin() || user.IsAdmin();
        }

        /// <summary>
        /// A generic extension method that aids in reflecting and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            var customAttribute = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();
            return customAttribute?.Name ?? enumValue.ToString();
        }

        public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
                return defaultValue;

            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }

        /// <summary>
        /// This method for get value one of Header properties.
        /// </summary>
        /// <param name="request">Request for Header properties.</param>
        /// <param name="key">Key of header property.</param>
        /// <returns>Return gets value of header property</returns>
        public static string GetHeaderValue(this HttpRequest request, string key = "x-auth-token") => request.Headers[key].ToString().Trim() ?? string.Empty;

        public static string GetBearerToken(this HttpContext context)
        {
            return context.Request.GetHeaderValue("Authorization").Replace("Bearer ", "");
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
    }
}
