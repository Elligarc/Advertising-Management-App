using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Core.Contracts;

public class UpdateSurfaceStatusData
{
    public Status Status { get; set; }
    public DateTime DateFrom { get; set; }
}