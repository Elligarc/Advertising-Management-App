using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class SurfaceStatus : IAggregateRoot
{
    private SurfaceStatus()
    {
    }

    public SurfaceStatus(Status status, DateTime dateFrom, int surfaceId)
    {
        Status = status;
        DateFrom = dateFrom;
        SurfaceId = surfaceId;
    }

    public int Id { get; set; }

    public Status Status { get; set; }

    public DateTime DateFrom { get; set; }

    public int SurfaceId { get; set; }

    public virtual Surface? Surface { get; set; }
}