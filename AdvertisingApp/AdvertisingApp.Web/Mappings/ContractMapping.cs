using AdvertisingApp.Core.Entities;
using AdvertisingApp.Web.Responses;

namespace AdvertisingApp.Web.Mappings;

public static class ContractMapping
{
    public static ContractResponseModel ToResponse(this Contract contract)
    {
        return new ContractResponseModel
        {
            Id = contract.Id,
            ClientId = contract.ClientId,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            TotalPrice = contract.TotalPrice,
            Status = contract.Status,
            ClientName = contract.Client?.Name,
            Items = contract.Items?.Select(i => i.ToResponse()).ToList() ?? new List<ContractItemResponseModel>()
        };
    }

    public static ContractItemResponseModel ToResponse(this ContractItem item)
    {
        return new ContractItemResponseModel
        {
            Id = item.Id,
            SurfaceId = item.SurfaceId,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Price = item.Price,
            PriceType = item.PriceType,
            TotalPrice = item.TotalPrice,
            // Schedule = item.Schedule?.ToResponse(),
            Surface = item.Surface?.ToBriefResponse()
        };
    }

    public static ContractItemScheduleResponseModel ToResponse(this ContractItemSchedule schedule)
    {
        return new ContractItemScheduleResponseModel
        {
            DaysOfWeek = schedule.DaysOfWeek ?? Array.Empty<int>(),
            HoursInDay = schedule.HoursInDay ?? Array.Empty<int>()
        };
    }

    public static SurfaceBriefResponseModel ToBriefResponse(this Surface surface)
    {
        return new SurfaceBriefResponseModel
        {
            Id = surface.Id,
            Side = surface.Side,
            SurfaceType = surface.SurfaceType,
            LoopDuration = surface.LoopDuration,
            SlotDuration = surface.SlotDuration
        };
    }
}