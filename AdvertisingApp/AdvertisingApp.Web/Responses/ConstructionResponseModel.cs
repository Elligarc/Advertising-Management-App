using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Web.Responses;

public class ConstructionResponseModel
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;

    public DistrictResponseModel? District { get; set; }
    public FormatResponseModel? Format { get; set; }
    public CityResponseModel? City { get; set; }
}

public class DistrictResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class FormatResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ConstructionType ConstructionType { get; set; }
}

public class CityResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}