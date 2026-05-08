using ProductCatalogApi.DTOs;
using ProductCatalogApi.Models.Entities;

namespace ProductCatalogApi.Interfaces.Services;

public interface IProvinceService
{
    Task<ApiResponse<List<Province>>> GetProvincesAsync();
}
