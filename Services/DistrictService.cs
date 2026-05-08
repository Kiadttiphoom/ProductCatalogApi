using Microsoft.Extensions.Caching.Memory;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Services;



public class DistrictService : IDistrictService
{
    private readonly IDistrictRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DistrictService> _logger;

    public DistrictService(
        IDistrictRepository repository,
        IMemoryCache cache,
        ILogger<DistrictService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<ApiResponse<List<District>>> GetDistrictsAsync()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        const string cacheKey = "district_list";

        try
        {
            if (!_cache.TryGetValue(cacheKey, out List<District>? districts))
            {
                districts = await _repository.GetAllDistrictsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, districts, cacheOptions);
                _logger.LogInformation("Districts loaded from database and cached");
            }
            else
            {
                _logger.LogInformation("Districts retrieved from cache");
            }

            stopwatch.Stop();
            return ApiResponse<List<District>>.SuccessResponse(districts ?? new List<District>(), stopwatch.ElapsedMilliseconds);
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

            _logger.LogError(ex, "Error in GetDistrictsAsync");
            return ApiResponse<List<District>>.ErrorResponse(message, statusCode: statusCode);
        }
    }
}
