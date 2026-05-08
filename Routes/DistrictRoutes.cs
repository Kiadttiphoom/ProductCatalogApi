using Asp.Versioning;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

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

    private static async Task<IResult> GetDistricts(IDistrictService districtService)
    {
        var result = await districtService.GetDistrictsAsync();
        return result.Success ? Results.Ok(result) : Results.Json(result, statusCode: result.StatusCode);
    }
}
