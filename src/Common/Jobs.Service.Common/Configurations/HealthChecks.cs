using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Jobs.Service.Common.Infrastructure.HealthChecks;
using System.Threading.Tasks;

namespace Jobs.Service.Common.Configurations
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddJobsHealthChecks(this IServiceCollection services)
        {
            var healthChecks = services.AddHealthChecks();
            healthChecks.AddCheck<MemoryHealthCheck>("Memory");

            return healthChecks;
        }

        public static void MapJobsHealthChecks(this IEndpointRouteBuilder endpoints, string uiPath = "/hc")
        {
            endpoints.MapHealthChecks(uiPath, new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse
            });

            static Task WriteResponse(HttpContext context, HealthReport result)
            {
                var responce = @"<p style='text-align:center; font-size: 32px;'>Health checks</p>
                     <table style='width:100%; text-align:center;'>
                       <tr>
                        <th>NAME</th>
                        <th>HEALTH</th>
                        <th>DESCRIPTION</th>
                        <th>DURATION</th>
                      </tr>";

                foreach (var entry in result.Entries)
                {
                    responce += "<tr>";
                    responce += $"<td>{entry.Key}</td>";
                    responce += $"<td>{entry.Value.Status}</td>";
                    responce += $"<td>{entry.Value.Description}</td>";
                    responce += $"<td>{entry.Value.Duration}</td>";
                    responce += "</tr>";
                }
                responce += "</table>";

                return context.Response.WriteAsync(responce);
            }
        }
    }
}
