using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Web.Responses;

public class ContractResponseModel
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public ContractStatus Status { get; set; }

    public string? ClientName { get; set; }
    public List<ContractItemResponseModel> Items { get; set; } = new();
}

public class ContractItemResponseModel
{
    public int Id { get; set; }
    public int SurfaceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public PriceType PriceType { get; set; }
    public decimal TotalPrice { get; set; }

    // public ContractItemScheduleResponseModel? Schedule { get; set; }
    public SurfaceBriefResponseModel? Surface { get; set; }
}

public class ContractItemScheduleResponseModel
{
    public int[] DaysOfWeek { get; set; } = Array.Empty<int>();
    public int[] HoursInDay { get; set; } = Array.Empty<int>();
}

public class SurfaceBriefResponseModel
{
    public int Id { get; set; }
    public Side Side { get; set; }
    public SurfaceType SurfaceType { get; set; }
    public int? LoopDuration { get; set; }
    public int? SlotDuration { get; set; }
}