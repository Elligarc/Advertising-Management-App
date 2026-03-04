using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Core.Contracts;

public class CreateContractItemData
{
    public int SurfaceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public PriceType PriceType { get; set; }

    // Для ContractItemSchedule (только для Digital поверхностей)
    public int[]? DaysOfWeek { get; set; }
    public int[]? HoursInDay { get; set; }
}