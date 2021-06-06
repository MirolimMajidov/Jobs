using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.HealthChecks
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public SqlServerHealthCheck(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Execute health check logic here.
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1;";
                    await command.ExecuteScalarAsync(cancellationToken);
                }

                return HealthCheckResult.Healthy("");
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}
