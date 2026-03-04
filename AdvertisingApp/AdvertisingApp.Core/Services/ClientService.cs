using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Services;

public class ClientService : IClientService
{
    private readonly IRepository<Client> _clientRepository;

    public ClientService(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _clientRepository.ListAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task<Client> CreateAsync(CreateClientData data)
    {
        var client = new Client(data.Name, data.Phone);

        await _clientRepository.AddAsync(client);
        return client;
    }

    public async Task<Client> UpdateAsync(int id, UpdateClientData data)
    {
        var existingClient = await _clientRepository.GetByIdAsync(id);
        if (existingClient == null)
            throw new KeyNotFoundException($"Client with id {id} not found");

        existingClient.Update(data.Name, data.Phone);

        await _clientRepository.UpdateAsync(existingClient);
        return existingClient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
            return false;

        await _clientRepository.DeleteAsync(client);
        return true;
    }
}