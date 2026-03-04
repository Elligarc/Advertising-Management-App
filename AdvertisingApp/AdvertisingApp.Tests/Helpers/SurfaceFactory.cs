using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Infrastructure.Data;

namespace AdvertisingApp.Tests.Helpers;

public static class SurfaceFactory
{
    public static Surface Create(Side side = Side.A, SurfaceType surfaceType = SurfaceType.Digital,
        int constructionId = 1, int loopDuration = 60, int slotDuration = 10)
    {
        return new Surface(side, surfaceType, constructionId, loopDuration, slotDuration);
    }

    public static async Task<Surface> CreateAndSaveAsync(
        AppDbContext context,
        Side side = Side.A,
        SurfaceType surfaceType = SurfaceType.Digital,
        string address = "Test Address",
        string cityName = "Test City",
        string districtName = "Test District",
        string formatName = "Billboard",
        ConstructionType constructionType = ConstructionType.Billboard,
        int loopDuration = 60,
        int slotDuration = 10,
        decimal price = 100m,
        PriceType priceType = PriceType.PerShow,
        Status status = Status.Created,
        DateTime? priceDate = null,
        DateTime? statusDate = null)
    {
        // Create full chain: City -> District -> Format -> Construction
        var (_, _, _, construction) = await ConstructionFactory.CreateFullChainAsync(
            context, address, cityName, districtName, formatName, constructionType);

        // Create surface
        var surface = new Surface(side, surfaceType, construction.Id, loopDuration, slotDuration);
        await context.Surfaces.AddAsync(surface);

        // Create price list
        var priceDateValue = priceDate ?? DateTime.UtcNow;
        var priceList = new PriceList(priceType, price, priceDateValue, surface.Id);
        await context.PriceLists.AddAsync(priceList);

        // Create status
        var statusDateValue = statusDate ?? DateTime.UtcNow;
        var surfaceStatus = new SurfaceStatus(status, statusDateValue, surface.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus);

        await context.SaveChangesAsync();

        return surface;
    }

    public static async Task<Surface> CreateWithBookingAsync(
        AppDbContext context,
        Side side = Side.A,
        SurfaceType surfaceType = SurfaceType.Digital,
        string address = "Test Address",
        string cityName = "Test City",
        string districtName = "Test District",
        string formatName = "Billboard",
        ConstructionType constructionType = ConstructionType.Billboard,
        int loopDuration = 60,
        int slotDuration = 10,
        decimal price = 100m,
        PriceType priceType = PriceType.PerShow,
        Status status = Status.Created,
        DateTime? bookingDate = null,
        int slotsOccupied = 0)
    {
        // Create full chain: City -> District -> Format -> Construction
        var (_, _, _, construction) = await ConstructionFactory.CreateFullChainAsync(
            context, address, cityName, districtName, formatName, constructionType);

        // Create surface
        var surface = new Surface(side, surfaceType, construction.Id, loopDuration, slotDuration);
        await context.Surfaces.AddAsync(surface);

        // Create price list
        var priceList = new PriceList(priceType, price, DateTime.UtcNow.AddDays(-1), surface.Id);
        await context.PriceLists.AddAsync(priceList);

        // Create status
        var surfaceStatus = new SurfaceStatus(status, DateTime.UtcNow.AddDays(-1), surface.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus);

        // Create booking if specified
        if (slotsOccupied > 0)
        {
            var bookingDateValue = bookingDate ?? DateTime.UtcNow.AddDays(1);
            var booking = new SurfaceBooking(bookingDateValue, 0, slotsOccupied, surface.Id);
            await context.SurfaceBookings.AddAsync(booking);
        }

        await context.SaveChangesAsync();

        return surface;
    }

    public static async Task<List<Surface>> CreateMultipleWithDifferentCitiesAsync(
        AppDbContext context,
        string city1Name,
        string city2Name,
        Side side = Side.A,
        SurfaceType surfaceType = SurfaceType.Digital,
        decimal price1 = 100m,
        decimal price2 = 200m)
    {
        // Create city 1
        var city1 = new City(city1Name);
        await context.Cities.AddAsync(city1);

        // Create district 1
        var district1 = new District("Central District", city1.Id);
        await context.Districts.AddAsync(district1);

        // Create format
        var format = new ConstructionFormat("Billboard", ConstructionType.Billboard);
        await context.ConstructionFormats.AddAsync(format);

        // Create construction 1
        var construction1 = new Construction("Address 1", district1.Id, format.Id);
        await context.Constructions.AddAsync(construction1);

        // Create surface 1
        var surface1 = new Surface(side, surfaceType, construction1.Id, 60, 10);
        await context.Surfaces.AddAsync(surface1);

        var priceList1 = new PriceList(PriceType.PerShow, price1, DateTime.UtcNow, surface1.Id);
        await context.PriceLists.AddAsync(priceList1);

        var surfaceStatus1 = new SurfaceStatus(Status.Created, DateTime.UtcNow, surface1.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus1);

        // Create city 2
        var city2 = new City(city2Name);
        await context.Cities.AddAsync(city2);

        // Create district 2
        var district2 = new District("Lenin District", city2.Id);
        await context.Districts.AddAsync(district2);

        // Create construction 2
        var construction2 = new Construction("Address 2", district2.Id, format.Id);
        await context.Constructions.AddAsync(construction2);

        // Create surface 2
        var surface2 = new Surface(side, surfaceType, construction2.Id, 60, 10);
        await context.Surfaces.AddAsync(surface2);

        var priceList2 = new PriceList(PriceType.PerShow, price2, DateTime.UtcNow, surface2.Id);
        await context.PriceLists.AddAsync(priceList2);

        var surfaceStatus2 = new SurfaceStatus(Status.Created, DateTime.UtcNow, surface2.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus2);

        await context.SaveChangesAsync();

        return new List<Surface> { surface1, surface2 };
    }

    public static async Task<List<Surface>> CreateMultipleWithDifferentPriceTypesAsync(
        AppDbContext context,
        PriceType priceType1,
        PriceType priceType2,
        decimal price1 = 100m,
        decimal price2 = 5000m)
    {
        // Create city
        var city = new City("Moscow");
        await context.Cities.AddAsync(city);

        // Create district
        var district = new District("Central District", city.Id);
        await context.Districts.AddAsync(district);

        // Create format
        var format = new ConstructionFormat("Billboard", ConstructionType.Billboard);
        await context.ConstructionFormats.AddAsync(format);

        // Create construction
        var construction = new Construction("Address 1", district.Id, format.Id);
        await context.Constructions.AddAsync(construction);

        // Create surface 1
        var surface1 = new Surface(Side.A, SurfaceType.Digital, construction.Id, 60, 10);
        await context.Surfaces.AddAsync(surface1);

        var priceList1 = new PriceList(priceType1, price1, DateTime.UtcNow.AddDays(-1), surface1.Id);
        await context.PriceLists.AddAsync(priceList1);

        var surfaceStatus1 = new SurfaceStatus(Status.Created, DateTime.UtcNow.AddDays(-1), surface1.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus1);

        // Create surface 2
        var surface2 = new Surface(Side.B, SurfaceType.Digital, construction.Id, 60, 10);
        await context.Surfaces.AddAsync(surface2);

        var priceList2 = new PriceList(priceType2, price2, DateTime.UtcNow.AddDays(-1), surface2.Id);
        await context.PriceLists.AddAsync(priceList2);

        var surfaceStatus2 = new SurfaceStatus(Status.Created, DateTime.UtcNow.AddDays(-1), surface2.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus2);

        await context.SaveChangesAsync();

        return new List<Surface> { surface1, surface2 };
    }

    public static async Task<List<Surface>> CreateMultipleWithDifferentConstructionTypesAsync(
        AppDbContext context,
        ConstructionType constructionType1,
        ConstructionType constructionType2)
    {
        // Create city
        var city = new City("Moscow");
        await context.Cities.AddAsync(city);

        // Create district
        var district = new District("Central District", city.Id);
        await context.Districts.AddAsync(district);

        // Create format 1
        var format1 = new ConstructionFormat("Billboard", constructionType1);
        await context.ConstructionFormats.AddAsync(format1);

        // Create format 2
        var format2 = new ConstructionFormat("Videoboard", constructionType2);
        await context.ConstructionFormats.AddAsync(format2);

        // Create construction 1
        var construction1 = new Construction("Address 1", district.Id, format1.Id);
        await context.Constructions.AddAsync(construction1);

        // Create surface 1
        var surface1 = new Surface(Side.A, SurfaceType.Digital, construction1.Id, 60, 10);
        await context.Surfaces.AddAsync(surface1);

        var priceList1 = new PriceList(PriceType.PerShow, 100m, DateTime.UtcNow, surface1.Id);
        await context.PriceLists.AddAsync(priceList1);

        var surfaceStatus1 = new SurfaceStatus(Status.Created, DateTime.UtcNow, surface1.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus1);

        // Create construction 2
        var construction2 = new Construction("Address 2", district.Id, format2.Id);
        await context.Constructions.AddAsync(construction2);

        // Create surface 2
        var surface2 = new Surface(Side.A, SurfaceType.Digital, construction2.Id, 60, 10);
        await context.Surfaces.AddAsync(surface2);

        var priceList2 = new PriceList(PriceType.PerShow, 200m, DateTime.UtcNow, surface2.Id);
        await context.PriceLists.AddAsync(priceList2);

        var surfaceStatus2 = new SurfaceStatus(Status.Created, DateTime.UtcNow, surface2.Id);
        await context.SurfaceStatuses.AddAsync(surfaceStatus2);

        await context.SaveChangesAsync();

        return new List<Surface> { surface1, surface2 };
    }
}