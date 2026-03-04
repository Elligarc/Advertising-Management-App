using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class Contract : IAggregateRoot
{
    private Contract()
    {
    }

    public Contract(int clientId, decimal totalPrice)
    {
        if (totalPrice < 0)
            throw new ArgumentException("Total price cannot be negative");

        ClientId = clientId;
        TotalPrice = totalPrice;
        Status = ContractStatus.Created;
    }

    public Contract(int clientId, DateTime startDate, DateTime endDate, decimal totalPrice)
        : this(clientId, totalPrice)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");

        StartDate = startDate;
        EndDate = endDate;
    }

    public int Id { get; set; }

    public int ClientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal TotalPrice { get; set; }

    public ContractStatus Status { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<ContractItem> Items { get; set; } = new List<ContractItem>();
}
