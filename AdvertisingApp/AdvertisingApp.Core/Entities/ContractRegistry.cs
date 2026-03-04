using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class ContractRegistry : IAggregateRoot
{
    private ContractRegistry()
    {
    }

    public ContractRegistry(int contractItemId, int contractId, DateTime contractStartDate, DateTime contractEndDate,
        decimal contractTotalPrice, ContractStatus contractStatus, DateTime itemStartDate, DateTime itemEndDate,
        decimal itemPrice, PriceType itemPriceType, decimal itemTotalPrice, int surfaceId, int clientId,
        string clientName, string clientPhone)
    {
        ContractItemId = contractItemId;
        ContractId = contractId;
        ContractStartDate = contractStartDate;
        ContractEndDate = contractEndDate;
        ContractTotalPrice = contractTotalPrice;
        ContractStatus = contractStatus;
        ItemStartDate = itemStartDate;
        ItemEndDate = itemEndDate;
        ItemPrice = itemPrice;
        ItemPriceType = itemPriceType;
        ItemTotalPrice = itemTotalPrice;
        SurfaceId = surfaceId;
        ClientId = clientId;
        ClientName = clientName ?? string.Empty;
        ClientPhone = clientPhone ?? string.Empty;
    }

    public int Id { get; set; }

    public int ContractItemId { get; set; }

    // Fields from Contract
    public int ContractId { get; set; }
    public DateTime ContractStartDate { get; set; }
    public DateTime ContractEndDate { get; set; }
    public decimal ContractTotalPrice { get; set; }
    public ContractStatus ContractStatus { get; set; }

    // Fields from ContractItem
    public DateTime ItemStartDate { get; set; }
    public DateTime ItemEndDate { get; set; }
    public decimal ItemPrice { get; set; }
    public PriceType ItemPriceType { get; set; }
    public decimal ItemTotalPrice { get; set; }

    // Fields from Surface
    public int SurfaceId { get; set; }

    // Fields from Client
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;

    public virtual ContractItem? ContractItem { get; set; }
}