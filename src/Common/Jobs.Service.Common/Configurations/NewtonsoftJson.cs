using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jobs.Service.Common.Configurations
{
    public static class NewtonsoftJsonExtensions
    {
        public static void AddResponseNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             });
        }
    }
}
