using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class PriceList : IAggregateRoot
{
    private PriceList()
    {
    }

    public PriceList(PriceType priceType, decimal price, DateTime dateFrom, int surfaceId)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        PriceType = priceType;
        Price = price;
        DateFrom = dateFrom;
        SurfaceId = surfaceId;
    }

    public int Id { get; set; }

    public PriceType PriceType { get; set; }

    public decimal Price { get; set; }

    public DateTime DateFrom { get; set; }

    public int SurfaceId { get; set; }

    public virtual Surface? Surface { get; set; }
}