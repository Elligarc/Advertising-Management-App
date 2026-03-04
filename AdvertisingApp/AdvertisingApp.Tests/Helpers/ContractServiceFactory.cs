using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;

namespace AdvertisingApp.Tests.Helpers;

public static class ContractServiceFactory
{
    public static (IContractService Service, AppDbContext Context) Create(AppDbContext context)
    {
        var contractRepo = new EfRepository<Contract>(context);
        var surfaceRepo = new EfRepository<Surface>(context);
        var surfaceBookingRepo = new EfRepository<SurfaceBooking>(context);
        var clientRepo = new EfRepository<Client>(context);

        var service = new ContractService(contractRepo, surfaceRepo, surfaceBookingRepo, clientRepo);
        return (service, context);
    }
}