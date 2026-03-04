using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Core.Contracts;

public class CreateSurfaceData
{
    public int ConstructionId { get; set; }
    public Side Side { get; set; }
    public SurfaceType SurfaceType { get; set; }
    public int? LoopDuration { get; set; }
    public int? SlotDuration { get; set; }
    public decimal InitialPrice { get; set; }
    public PriceType PriceType { get; set; }
}