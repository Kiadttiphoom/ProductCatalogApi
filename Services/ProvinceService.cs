using Microsoft.Extensions.Caching.Memory;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Services;



public class ProvinceService : IProvinceService
{
    private readonly IProvinceRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProvinceService> _logger;

    public ProvinceService(
        IProvinceRepository repository,
        IMemoryCache cache,
        ILogger<ProvinceService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ApiResponse<List<Province>>> GetProvincesAsync()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        const string cacheKey = "province_list";

        try
        {
            if (!_cache.TryGetValue(cacheKey, out List<Province>? provinces))
            {
                provinces = await _repository.GetAllProvincesAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, provinces, cacheOptions);
                _logger.LogInformation("Provinces loaded from database and cached");
            }
            else
            {
                _logger.LogInformation("Provinces retrieved from cache");
            }

            stopwatch.Stop();
            return ApiResponse<List<Province>>.SuccessResponse(provinces ?? new List<Province>(), stopwatch.ElapsedMilliseconds);
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

            _logger.LogError(ex, "Error in GetProvincesAsync");
            return ApiResponse<List<Province>>.ErrorResponse(message, statusCode: statusCode);
        }
    }
}
