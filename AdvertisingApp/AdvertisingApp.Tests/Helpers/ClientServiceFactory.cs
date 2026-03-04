using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;
using CoreEntities = AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Tests.Helpers;

public static class ClientServiceFactory
{
    public static (IClientService Service, AppDbContext Context) Create(AppDbContext context)
    {
        var repository = new EfRepository<CoreEntities.Client>(context);
        var service = new ClientService(repository);
        return (service, context);
    }
}