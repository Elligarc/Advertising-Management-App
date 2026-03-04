using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class SurfaceBooking : IAggregateRoot
{
    private SurfaceBooking()
    {
    }

    public SurfaceBooking(DateTime date, int hour, int slotsOccupied, int surfaceId)
    {
        if (hour < 0 || hour > 23)
            throw new ArgumentException("Hour must be between 0 and 23", nameof(hour));

        if (slotsOccupied < 0)
            throw new ArgumentException("Slots occupied cannot be negative", nameof(slotsOccupied));

        Date = date;
        Hour = hour;
        SlotsOccupied = slotsOccupied;
        SurfaceId = surfaceId;
    }

    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Hour { get; set; }

    public int SlotsOccupied { get; set; }

    public int SurfaceId { get; set; }

    public virtual Surface? Surface { get; set; }
}