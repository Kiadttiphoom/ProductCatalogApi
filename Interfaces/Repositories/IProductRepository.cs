using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
}
