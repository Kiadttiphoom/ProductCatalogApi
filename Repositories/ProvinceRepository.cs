using Dapper;
using Microsoft.Data.SqlClient;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Repositories;



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
        catch (SqlException ex) when (ex.Number == 208)
        {
            _logger.LogWarning("❌ Database table not found: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching provinces");
            throw;
        }
    }
}
