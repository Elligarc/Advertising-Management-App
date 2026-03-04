using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class ContractItem : IAggregateRoot
{
    private ContractItem()
    {
    }

    public ContractItem(int surfaceId, int contractId, DateTime startDate, DateTime endDate, decimal price,
        PriceType priceType)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");

        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        SurfaceId = surfaceId;
        ContractId = contractId;
        StartDate = startDate;
        EndDate = endDate;
        Price = price;
        PriceType = priceType;
        TotalPrice = price;
    }

    public int Id { get; set; }

    public int SurfaceId { get; set; }

    public int ContractId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public PriceType PriceType { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Surface? Surface { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ContractItemSchedule? Schedule { get; set; }

    public virtual ContractRegistry? Registry { get; set; }
}