using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Core.Interfaces;

public interface ISurfaceService
{
    Task<IEnumerable<Surface>> GetByConstructionIdAsync(int constructionId);
    Task<Surface?> GetByIdAsync(int id);
    Task<IEnumerable<Surface>> GetFilteredAsync(FilterSurfacesData filter);
    Task<Surface> CreateAsync(CreateSurfaceData data);
    Task<Surface> UpdateAsync(int id, UpdateSurfaceData data);
    Task<Surface> UpdatePriceAsync(int id, UpdateSurfacePriceData data);
    Task<Surface> UpdateStatusAsync(int id, UpdateSurfaceStatusData data);
    Task<bool> DeleteAsync(int id);
}