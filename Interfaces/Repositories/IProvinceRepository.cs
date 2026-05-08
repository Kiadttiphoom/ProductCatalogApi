using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Repositories;

public interface IProvinceRepository
{
    Task<List<Province>> GetAllProvincesAsync();
}
