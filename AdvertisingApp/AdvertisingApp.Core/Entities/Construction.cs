using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class Construction : IAggregateRoot
{
    private Construction()
    {
    }

    public Construction(string address, int districtId, int formatId)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        Address = address;
        DistrictId = districtId;
        FormatId = formatId;
    }

    public int Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public int DistrictId { get; set; }

    public int FormatId { get; set; }

    public virtual District? District { get; set; }

    public virtual ConstructionFormat? Format { get; set; }

    public virtual ICollection<Surface> Surfaces { get; set; } = new List<Surface>();

    public void Update(string address, int districtId, int formatId)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        Address = address;
        DistrictId = districtId;
        FormatId = formatId;
    }
}