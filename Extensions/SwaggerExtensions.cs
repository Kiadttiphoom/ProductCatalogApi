using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace ProductCatalogApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services)
    {
        // Required for Minimal API + Swagger
        services.AddEndpointsApiExplorer();

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ProductCatalogApi - Shop API",
                Version = "v1.0",
                Description = "REST API for managing products, provinces, and districts"
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "ProductCatalogApi v1");

                options.RoutePrefix = string.Empty;
            });
        }

        return app;
    }
}