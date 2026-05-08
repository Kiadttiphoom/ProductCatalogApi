using Dapper;
using Microsoft.Data.SqlClient;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Models.Entities;

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
            var sql = "SELECT * FROM products";
            var products = await connection.QueryAsync<Product>(sql);
            return products.ToList();
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            _logger.LogWarning("❌ Database table not found: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products");
            throw;
        }
    }
}
