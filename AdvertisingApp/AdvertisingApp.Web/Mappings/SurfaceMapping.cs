using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Web.Responses;

namespace AdvertisingApp.Web.Mappings;

public static class SurfaceMapping
{
    public static SurfaceResponseModel ToResponse(this Surface surface)
    {
        var currentPrice = surface.PriceLists?.OrderByDescending(p => p.DateFrom).FirstOrDefault();
        var currentStatus = surface.Statuses?.OrderByDescending(s => s.DateFrom).FirstOrDefault();

        return new SurfaceResponseModel
        {
            Id = surface.Id,
            Side = surface.Side,
            SurfaceType = surface.SurfaceType,
            LoopDuration = surface.LoopDuration,
            SlotDuration = surface.SlotDuration,
            MaxSlots = surface.MaxSlots,
            // ConstructionId = surface.ConstructionId,
            CurrentPrice = currentPrice?.Price ?? 0,
            CurrentPriceType = currentPrice?.PriceType ?? PriceType.PerMonth,
            CurrentStatus = currentStatus?.Status ?? Status.Created,
            Construction = surface.Construction?.ToResponse()
        };
    }
}