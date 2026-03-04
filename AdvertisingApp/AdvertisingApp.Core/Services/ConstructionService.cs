using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Specifications;

namespace AdvertisingApp.Core.Services;

public class ConstructionService : IConstructionService
{
    private readonly IRepository<Construction> _constructionRepository;

    public ConstructionService(IRepository<Construction> constructionRepository)
    {
        _constructionRepository = constructionRepository;
    }

    public async Task<IEnumerable<Construction>> GetAllAsync()
    {
        return await _constructionRepository.ListAsync(new AllConstructionsWithDetailsSpecification());
    }

    public async Task<Construction?> GetByIdAsync(int id)
    {
        return await _constructionRepository.FirstOrDefaultAsync(new ConstructionByIdWithDetailsSpecification(id));
    }

    public async Task<Construction> CreateAsync(CreateConstructionData data)
    {
        var construction = new Construction(data.Address, data.DistrictId, data.FormatId);

        await _constructionRepository.AddAsync(construction);
        return construction;
    }

    public async Task<Construction> UpdateAsync(int id, UpdateConstructionData data)
    {
        var existingConstruction = await _constructionRepository.GetByIdAsync(id);
        if (existingConstruction == null)
            throw new KeyNotFoundException($"Construction with id {id} not found");

        existingConstruction.Update(data.Address, data.DistrictId, data.FormatId);

        await _constructionRepository.UpdateAsync(existingConstruction);
        return existingConstruction;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var construction = await _constructionRepository.GetByIdAsync(id);
        if (construction == null)
            return false;

        await _constructionRepository.DeleteAsync(construction);
        return true;
    }
}