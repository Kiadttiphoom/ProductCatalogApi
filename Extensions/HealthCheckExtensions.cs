using FluentValidation;
using ProductCatalogApi.Authentication;
using ProductCatalogApi.HealthChecks;
using ProductCatalogApi.Validators;

namespace ProductCatalogApi.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register health check services first
        services.AddScoped<DatabaseHealthCheck>();
        services.AddScoped<ApiHealthCheck>();

        services.AddHealthChecks()
            .AddCheck<ApiHealthCheck>("API")
            .AddCheck<DatabaseHealthCheck>("Database", tags: new[] { "database" });

        return services;
    }

    public static WebApplication UseHealthCheckConfiguration(this WebApplication app)
    {
        app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new
                        {
                            status = kvp.Value.Status.ToString(),
                            description = kvp.Value.Description
                        })
                });
                await context.Response.WriteAsync(json);
            }
        });

        return app;
    }
}
