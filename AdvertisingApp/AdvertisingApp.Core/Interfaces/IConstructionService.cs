using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Core.Interfaces;

public interface IConstructionService
{
    Task<IEnumerable<Construction>> GetAllAsync();
    Task<Construction?> GetByIdAsync(int id);
    Task<Construction> CreateAsync(CreateConstructionData data);
    Task<Construction> UpdateAsync(int id, UpdateConstructionData data);
    Task<bool> DeleteAsync(int id);
}