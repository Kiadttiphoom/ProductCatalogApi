using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Repositories;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add caching
        services.AddMemoryCache();

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register repositories
        services.AddScoped<IProductRepository>(sp =>
            new ProductRepository(connectionString!, sp.GetRequiredService<ILogger<ProductRepository>>()));
        services.AddScoped<IProvinceRepository>(sp =>
            new ProvinceRepository(connectionString!, sp.GetRequiredService<ILogger<ProvinceRepository>>()));
        services.AddScoped<IDistrictRepository>(sp =>
            new DistrictRepository(connectionString!, sp.GetRequiredService<ILogger<DistrictRepository>>()));

        // Register services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<IDistrictService, DistrictService>();

        return services;
    }
}
