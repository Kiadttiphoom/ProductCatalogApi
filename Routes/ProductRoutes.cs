using Asp.Versioning;
using ProductCatalogApi.DTOs;
using ProductCatalogApi.Interfaces.Services;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Routes;

public static class ProductRoutes
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var api = app.NewVersionedApi("API v1");

        api.MapGet("/products", GetProducts)
           .WithName("GetProducts")
           .Produces<ApiResponse<PaginatedResponse<Product>>>(StatusCodes.Status200OK)
           .Produces<object>(StatusCodes.Status400BadRequest)
           .Produces<object>(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> GetProducts(IProductService productService)
    {
        var result = await productService.GetProductsAsync();
        return result.Success ? Results.Ok(result) : Results.Json(result, statusCode: result.StatusCode);
    }
}
