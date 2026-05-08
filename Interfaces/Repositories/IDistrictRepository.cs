using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Repositories;

public interface IDistrictRepository
{
    Task<List<District>> GetAllDistrictsAsync();
}
