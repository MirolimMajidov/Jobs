using Durak.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace JobService.Configurations
{
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        #region ISchemaFilter Members

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>() != null).Select(p => p.Name.ToCamelCase());

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty))
                    schema.Properties.Remove(excludedProperty);
            }
        }

        #endregion
    }
}
