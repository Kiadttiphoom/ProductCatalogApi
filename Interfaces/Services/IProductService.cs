using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Services;

public interface IProductService
{
    Task<ApiResponse<List<Product>>> GetProductsAsync();
}
