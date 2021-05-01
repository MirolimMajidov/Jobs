using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Service.SharedModel.Configurations
{
    public static class ResponseJsonExtensions
    {
        public static void AddResponseJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                 options.JsonSerializerOptions.IgnoreNullValues = true;
                 options.JsonSerializerOptions.WriteIndented = true;
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
        }
    }
}
