namespace ProductCatalogApi.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication UseHealthCheck(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        try
        {
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
            logger.LogInformation("✅ Database Connection: Success!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Database Connection: Failed!");
            // Uncomment to fail fast:
            // throw;
        }

        return app;
    }
}
