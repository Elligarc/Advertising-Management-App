using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Infrastructure.Data;

namespace AdvertisingApp.Tests.Helpers;

public static class ConstructionFactory
{
    public static Construction Create(string address = "Test Address", int districtId = 1, int formatId = 1)
    {
        return new Construction(address, districtId, formatId);
    }

    public static async Task<(City City, District District, ConstructionFormat Format, Construction Construction)>
        CreateFullChainAsync(
            AppDbContext context,
            string address = "Test Address",
            string cityName = "Test City",
            string districtName = "Test District",
            string formatName = "Billboard",
            ConstructionType constructionType = ConstructionType.Billboard)
    {
        var city = new City(cityName);
        await context.Cities.AddAsync(city);

        var district = new District(districtName, city.Id);
        await context.Districts.AddAsync(district);

        var format = new ConstructionFormat(formatName, constructionType);
        await context.ConstructionFormats.AddAsync(format);

        var construction = new Construction(address, district.Id, format.Id);
        await context.Constructions.AddAsync(construction);

        await context.SaveChangesAsync();

        return (city, district, format, construction);
    }
}