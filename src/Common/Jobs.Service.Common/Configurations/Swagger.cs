using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Jobs.Service.Common.Configurations
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerGen(this IServiceCollection services, string serviceName, string docName = "v1", string versionName = "v1")
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc(docName, new OpenApiInfo { Title = $"{serviceName} service's RESTful APIs documentation", Version = versionName }); c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.  Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
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
