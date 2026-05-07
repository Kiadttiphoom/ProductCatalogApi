using Microsoft.Extensions.Caching.Memory;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;
using ProductCatalogApi.Repositories;

namespace ProductCatalogApi.Services;

public interface IDataService
{
    Task<ApiResponse<List<Product>>> GetProductsAsync();
    Task<ApiResponse<List<Province>>> GetProvincesAsync();
    Task<ApiResponse<List<District>>> GetDistrictsAsync();
}

public class DataService : IDataService
{
    private readonly IProductRepository _productRepository;
    private readonly IProvinceRepository _provinceRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DataService> _logger;

    public DataService(
        IProductRepository productRepository,
        IProvinceRepository provinceRepository,
        IDistrictRepository districtRepository,
        IMemoryCache cache,
        ILogger<DataService> logger)
    {
        _productRepository = productRepository;
        _provinceRepository = provinceRepository;
        _districtRepository = districtRepository;
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
                products = await _productRepository.GetAllProductsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, products, cacheOptions);
                _logger.LogInformation("products loaded from database and cached");
            }
            else
            {
                _logger.LogInformation("products retrieved from cache");
            }

            stopwatch.Stop();
            return ApiResponse<List<Product>>.SuccessResponse(products ?? new List<Product>(), stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetDistrictsAsync");
            stopwatch.Stop();
            return ApiResponse<List<Product>>.ErrorResponse($"Error: {ex.Message}", stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<ApiResponse<List<Province>>> GetProvincesAsync()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        const string cacheKey = "province_list";

        try
        {
            if (!_cache.TryGetValue(cacheKey, out List<Province>? provinces))
            {
                provinces = await _provinceRepository.GetAllProvincesAsync();

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
            _logger.LogError(ex, "Error in GetProvincesAsync");
            stopwatch.Stop();
            return ApiResponse<List<Province>>.ErrorResponse($"Error: {ex.Message}", stopwatch.ElapsedMilliseconds);
        }
    }

    public async Task<ApiResponse<List<District>>> GetDistrictsAsync()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        const string cacheKey = "district_list";

        try
        {
            if (!_cache.TryGetValue(cacheKey, out List<District>? districts))
            {
                districts = await _districtRepository.GetAllDistrictsAsync();

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
            _logger.LogError(ex, "Error in GetDistrictsAsync");
            stopwatch.Stop();
            return ApiResponse<List<District>>.ErrorResponse($"Error: {ex.Message}", stopwatch.ElapsedMilliseconds);
        }
    }
}
