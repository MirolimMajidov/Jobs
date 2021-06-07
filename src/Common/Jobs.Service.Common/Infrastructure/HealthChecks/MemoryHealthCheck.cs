using Humanizer.Bytes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jobs.Service.Common.Infrastructure.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryCheckOptions> _options;

        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            _options = options;
        }

        public string Name => "memory_check";

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var options = _options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var totalMemory = BytesToMegabytes(GC.GetTotalMemory(forceFullCollection: false));
            var status = (totalMemory < options.Threshold) ? HealthStatus.Healthy : context.Registration.FailureStatus;
            var description = $"Max support Megabytes is {options.Threshold}. Using: {totalMemory} MB; 0 garbage collection: {BytesToMegabytes(GC.CollectionCount(0))} MB; 1 garbage collection: {BytesToMegabytes(GC.CollectionCount(1))} MB; 2 garbage collection: {BytesToMegabytes(GC.CollectionCount(2))} MB;";

            return Task.FromResult(new HealthCheckResult(status, description: description, exception: null, data: null));
        }

        double BytesToMegabytes(long bytes) => Math.Round(ByteSize.FromBytes(bytes).Megabytes, 2);
    }

    public class MemoryCheckOptions
    {
        // Failure threshold (in Megabytes)
        public int Threshold { get; set; } = 10 * 1024;
    }
}