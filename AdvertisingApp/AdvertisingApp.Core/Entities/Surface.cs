using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class Surface : IAggregateRoot
{
    private Surface()
    {
    }

    public Surface(Side side, SurfaceType surfaceType, int constructionId, int? loopDuration = null,
        int? slotDuration = null)
    {
        Side = side;
        SurfaceType = surfaceType;
        ConstructionId = constructionId;
        LoopDuration = loopDuration;
        SlotDuration = slotDuration;
        CalculateMaxSlots();
    }

    public int Id { get; set; }

    public Side Side { get; set; }

    public SurfaceType SurfaceType { get; set; }

    public int? LoopDuration { get; set; }

    public int? SlotDuration { get; set; }

    public int MaxSlots { get; set; }

    public int ConstructionId { get; set; }

    public virtual Construction? Construction { get; set; }

    public virtual ICollection<SurfaceStatus> Statuses { get; set; } = new List<SurfaceStatus>();
    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();
    public virtual ICollection<SurfaceBooking> Bookings { get; set; } = new List<SurfaceBooking>();

    public void CalculateMaxSlots()
    {
        if (SurfaceType == SurfaceType.Digital && SlotDuration.HasValue && LoopDuration.HasValue && SlotDuration > 0)
            MaxSlots = LoopDuration.Value / SlotDuration.Value;
        else
            MaxSlots = 1;
    }
}