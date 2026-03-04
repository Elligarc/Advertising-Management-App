using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Infrastructure.Data;

namespace AdvertisingApp.Tests.Helpers;

public static class ContractFactory
{
    public static Contract Create(int clientId, DateTime? startDate = null, DateTime? endDate = null,
        decimal totalPrice = 0)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(1);
        var end = endDate ?? DateTime.UtcNow.AddMonths(1);
        return new Contract(clientId, start, end, totalPrice);
    }

    public static async Task<Contract> CreateAndSaveAsync(
        AppDbContext context,
        int clientId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal totalPrice = 0,
        ContractStatus status = ContractStatus.Created)
    {
        var contract = Create(clientId, startDate, endDate, totalPrice);
        contract.Status = status;
        await context.Contracts.AddAsync(contract);
        await context.SaveChangesAsync();
        return contract;
    }

    public static async Task<Contract> CreateWithSurfaceAsync(
        AppDbContext context,
        Side side = Side.A,
        SurfaceType surfaceType = SurfaceType.Regular,
        int? clientId = null,
        ContractStatus status = ContractStatus.Created,
        int loopDuration = 60,
        int slotDuration = 10,
        decimal price = 10000m,
        PriceType priceType = PriceType.PerMonth)
    {
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            context,
            side,
            surfaceType,
            price: price,
            priceType: priceType,
            loopDuration: loopDuration,
            slotDuration: slotDuration);

        var client = clientId.HasValue
            ? await context.Clients.FindAsync(clientId.Value)
            : await ClientFactory.CreateAndSaveAsync(context);

        if (!clientId.HasValue && client == null)
            client = await ClientFactory.CreateAndSaveAsync(context);

        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddMonths(1);

        var contract = new Contract(client!.Id, startDate, endDate, price) { Status = status };
        await context.Contracts.AddAsync(contract);

        var contractItem = new ContractItem(
            surface.Id,
            contract.Id,
            startDate,
            endDate,
            price,
            priceType);
        contractItem.TotalPrice = price;

        await context.ContractItems.AddAsync(contractItem);
        contract.TotalPrice = price;

        await context.SaveChangesAsync();
        return contract;
    }
}