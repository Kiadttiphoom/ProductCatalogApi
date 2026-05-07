using ProductCatalogApi.Extensions;
using ProductCatalogApi.Middleware;
using ProductCatalogApi.Routes;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.ConfigureSerilog();

    // Add services to the container
    builder.Services.AddControllersWithViews();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddSwaggerConfiguration();

    var app = builder.Build();

    // Use error handling middleware
    app.UseErrorHandling();

    // Configure the HTTP request pipeline
    app.UseSwaggerConfiguration();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCors("AllowAll");
    app.UseRouting();
    app.UseAuthorization();

    // Map MVC controller route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Health check database connection
    app.UseHealthCheck();

    // Map API endpoints
    app.MapProductEndpoints();
    app.MapProvinceEndpoints();
    app.MapDistrictEndpoints();

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


