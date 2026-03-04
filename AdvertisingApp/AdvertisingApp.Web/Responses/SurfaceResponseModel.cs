using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Web.Responses;

public class SurfaceResponseModel
{
    public int Id { get; set; }
    public Side Side { get; set; }
    public SurfaceType SurfaceType { get; set; }
    public int? LoopDuration { get; set; }
    public int? SlotDuration { get; set; }

    public int MaxSlots { get; set; }

    // public int ConstructionId { get; set; }
    public decimal CurrentPrice { get; set; }
    public PriceType CurrentPriceType { get; set; }
    public Status CurrentStatus { get; set; }

    public ConstructionResponseModel? Construction { get; set; }
}