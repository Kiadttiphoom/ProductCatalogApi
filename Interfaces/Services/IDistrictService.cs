using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Services;

public interface IDistrictService
{
    Task<ApiResponse<List<District>>> GetDistrictsAsync();
}
