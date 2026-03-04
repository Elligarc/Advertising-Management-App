using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Core.Interfaces;

public interface IContractService
{
    Task<Contract> CreateAsync(CreateContractData data);
    Task<ContractItem> AddItemAsync(int contractId, CreateContractItemData data);
    Task<Contract> UpdateAsync(int id, UpdateContractData data);
    Task<Contract?> GetByIdAsync(int id);
    Task<IEnumerable<Contract>> GetAllAsync();
}