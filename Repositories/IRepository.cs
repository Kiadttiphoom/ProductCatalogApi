using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
}

public interface IProvinceRepository
{
    Task<List<Province>> GetAllProvincesAsync();
}

public interface IDistrictRepository
{
    Task<List<District>> GetAllDistrictsAsync();
}
