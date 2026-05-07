using Asp.Versioning;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

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

    private static async Task<IResult> GetProvinces(IDataService dataService)
    {
        var result = await dataService.GetProvincesAsync();
        return result.Success ? Results.Ok(result) : Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}
