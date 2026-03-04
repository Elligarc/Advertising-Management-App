using AdvertisingApp.Core.Entities;
using AdvertisingApp.Infrastructure.Data;

namespace AdvertisingApp.Tests.Helpers;

public static class ClientFactory
{
    public static Client Create(string name = "Test Client", string phone = "1234567890")
    {
        return new Client(name, phone);
    }

    public static async Task<Client> CreateAndSaveAsync(AppDbContext context, string name = "Test Client",
        string phone = "1234567890")
    {
        var client = Create(name, phone);
        await context.Clients.AddAsync(client);
        await context.SaveChangesAsync();
        return client;
    }
}