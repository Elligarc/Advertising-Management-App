using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Core.Contracts;

public class UpdateSurfacePriceData
{
    public decimal Price { get; set; }
    public PriceType PriceType { get; set; }
    public DateTime? DateFrom { get; set; }
}