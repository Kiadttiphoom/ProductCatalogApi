using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProductCatalogApi.HealthChecks;

public class ApiHealthCheck : IHealthCheck
{
    private readonly ILogger<ApiHealthCheck> _logger;

    public ApiHealthCheck(ILogger<ApiHealthCheck> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("API health check: Healthy");
            return Task.FromResult(HealthCheckResult.Healthy("API is healthy and running"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API health check: Unhealthy");
            return Task.FromResult(HealthCheckResult.Unhealthy("API health check failed", ex));
        }
    }
}
