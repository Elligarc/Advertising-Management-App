using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;
using CoreEntities = AdvertisingApp.Core.Entities;

namespace AdvertisingApp.Tests.Helpers;

public static class SurfaceServiceFactory
{
    public static (ISurfaceService Service, AppDbContext Context) Create(AppDbContext context)
    {
        var constructionRepo = new EfRepository<CoreEntities.Construction>(context);
        var surfaceRepo = new EfRepository<CoreEntities.Surface>(context);
        var priceListRepo = new EfRepository<CoreEntities.PriceList>(context);
        var surfaceStatusRepo = new EfRepository<CoreEntities.SurfaceStatus>(context);
        var surfaceBookingRepo = new EfRepository<CoreEntities.SurfaceBooking>(context);

        var service = new SurfaceService(surfaceRepo, constructionRepo, priceListRepo, surfaceStatusRepo,
            surfaceBookingRepo);
        return (service, context);
    }
}