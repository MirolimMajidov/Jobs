using Jobs.SharedModel.Helpers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Service.SharedModel.Configurations
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
