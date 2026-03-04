using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Core.Interfaces;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(CreateClientData data);
    Task<Client> UpdateAsync(int id, UpdateClientData data);
    Task<bool> DeleteAsync(int id);
}