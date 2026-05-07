using Dapper;
using Microsoft.Data.SqlClient;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(string connectionString, ILogger<ProductRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Product";
            var products = await connection.QueryAsync<Product>(sql);
            return products.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products");
            throw;
        }
    }
}

public class ProvinceRepository : IProvinceRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ProvinceRepository> _logger;

    public ProvinceRepository(string connectionString, ILogger<ProvinceRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<List<Province>> GetAllProvincesAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Province";
            var provinces = await connection.QueryAsync<Province>(sql);
            return provinces.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching provinces");
            throw;
        }
    }
}

public class DistrictRepository : IDistrictRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DistrictRepository> _logger;

    public DistrictRepository(string connectionString, ILogger<DistrictRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<List<District>> GetAllDistrictsAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM District";
            var districts = await connection.QueryAsync<District>(sql);
            return districts.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching districts");
            throw;
        }
    }
}
