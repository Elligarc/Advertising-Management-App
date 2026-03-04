namespace AdvertisingApp.Core.Contracts;

public class UpdateConstructionData
{
    public string Address { get; set; } = string.Empty;
    public int DistrictId { get; set; }
    public int FormatId { get; set; }
}