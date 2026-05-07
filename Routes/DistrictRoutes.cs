using Asp.Versioning;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Routes;

public static class DistrictRoutes
{
    public static WebApplication MapDistrictEndpoints(this WebApplication app)
    {
        var api = app.NewVersionedApi("API v1");

        api.MapGet("/district", GetDistricts)
           .WithName("GetDistricts")
           .Produces<ApiResponse<List<District>>>(StatusCodes.Status200OK)
           .Produces<object>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> GetDistricts(IDataService dataService)
    {
        var result = await dataService.GetDistrictsAsync();
        return result.Success ? Results.Ok(result) : Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}
