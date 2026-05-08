using Microsoft.Extensions.Caching.Memory;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Services;



public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IProductRepository repository,
        IMemoryCache cache,
        ILogger<ProductService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ApiResponse<List<Product>>> GetProductsAsync()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        const string cacheKey = "product_list";

        try
        {
            if (!_cache.TryGetValue(cacheKey, out List<Product>? products))
            {
                products = await _repository.GetAllProductsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, products, cacheOptions);
                _logger.LogInformation("Products loaded from database and cached");
            }
            else
            {
                _logger.LogInformation("Products retrieved from cache");
            }

            stopwatch.Stop();
            return ApiResponse<List<Product>>.SuccessResponse(products ?? new List<Product>(), stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = $"Error: {ex.Message}";

            if (ex is Microsoft.Data.SqlClient.SqlException sqlEx && sqlEx.Number == 208)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "Requested data source not found";
            }
            else if (ex.InnerException is Microsoft.Data.SqlClient.SqlException innerSqlEx && innerSqlEx.Number == 208)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "Requested data source not found";
            }

            _logger.LogError(ex, "Error in GetProductsAsync");
            return ApiResponse<List<Product>>.ErrorResponse(message, statusCode: statusCode);
        }
    }
}
