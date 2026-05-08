using Dapper;
using Microsoft.Data.SqlClient;
using ProductCatalogApi.Interfaces.Repositories;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Repositories;

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
            var sql = "SELECT * FROM pms011";
            var districts = await connection.QueryAsync<District>(sql);
            return districts.ToList();
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            _logger.LogWarning("❌ Database table not found: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching districts");
            throw;
        }
    }
}
