using AdvertisingApp.Core.Entities;
using AdvertisingApp.Web.Responses;

namespace AdvertisingApp.Web.Mappings;

public static class ConstructionMapping
{
    public static ConstructionResponseModel ToResponse(this Construction construction)
    {
        return new ConstructionResponseModel
        {
            Id = construction.Id,
            Address = construction.Address,
            District = construction.District != null
                ? new DistrictResponseModel
                {
                    Id = construction.District.Id,
                    Name = construction.District.Name
                }
                : null,
            Format = construction.Format != null
                ? new FormatResponseModel
                {
                    Id = construction.Format.Id,
                    Name = construction.Format.Name,
                    ConstructionType = construction.Format.ConstructionType
                }
                : null,
            City = construction.District?.City != null
                ? new CityResponseModel
                {
                    Id = construction.District.City.Id,
                    Name = construction.District.City.Name
                }
                : null
        };
    }
}