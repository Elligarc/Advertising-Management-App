using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;
using CoreEntities = AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Tests.Helpers;

public static class ConstructionServiceFactory
{
    public static (IConstructionService Service, AppDbContext Context) Create(AppDbContext context)
    {
        var repository = new EfRepository<CoreEntities.Construction>(context);
        var service = new ConstructionService(repository);
        return (service, context);
    }
}