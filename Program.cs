using ProductCatalogApi.Extensions;
using ProductCatalogApi.Middleware;
using ProductCatalogApi.Routes;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.ConfigureSerilog();

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddValidationConfiguration();
    builder.Services.AddHealthCheckConfiguration(builder.Configuration);

    var app = builder.Build();

    // Use error handling middleware
    app.UseErrorHandling();
    app.UseApiKey();

    // Configure the HTTP request pipeline
    app.UseSwaggerConfiguration();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    
    // Add authentication middleware
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.UseRouting();

    // Health check database connection
    app.UseHealthCheck();
    app.UseHealthCheckConfiguration();

    // Map API endpoints
    app.MapProductEndpoints();
    app.MapProvinceEndpoints();
    app.MapDistrictEndpoints();

    // Fallback for 404 Not Found
    app.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            var response = ProductCatalogApi.DTOs.ApiResponse<object>.ErrorResponse(
                "The requested endpoint was not found.", 
                statusCode: StatusCodes.Status404NotFound);
            
            await context.Response.WriteAsJsonAsync(response);
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Serilog.Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Serilog.Log.CloseAndFlush();
}



