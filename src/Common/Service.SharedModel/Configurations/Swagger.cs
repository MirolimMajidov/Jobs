using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Service.SharedModel.Configurations
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerGen(this IServiceCollection services, string serviceName, string docName = "v1", string versionName = "v1")
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc(docName, new OpenApiInfo { Title = $"{serviceName} service's RESTful APIs documentation", Version = versionName });
            });
        }

        public static void UseSwaggerDocs(this IApplicationBuilder app)
        {
            app.UseSwagger(o => { o.RouteTemplate = "docs/{documentName}/docs.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/docs/v1/docs.json", "RESTful APIs v1");
                c.RoutePrefix = "docs";
            });
        }
    }
}
