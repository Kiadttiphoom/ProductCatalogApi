using Asp.Versioning;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Routes;

public static class ProvinceRoutes
{
    public static WebApplication MapProvinceEndpoints(this WebApplication app)
    {
        var api = app.NewVersionedApi("API v1");

        api.MapGet("/province", GetProvinces)
           .WithName("GetProvinces")
           .Produces<ApiResponse<PaginatedResponse<Province>>>(StatusCodes.Status200OK)
           .Produces<object>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> GetProvinces(IProvinceService provinceService)
    {
        var result = await provinceService.GetProvincesAsync();
        return result.Success ? Results.Ok(result) : Results.Json(result, statusCode: result.StatusCode);
    }
}
