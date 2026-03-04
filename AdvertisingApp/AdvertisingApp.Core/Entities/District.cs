using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class District : IAggregateRoot
{
    private District()
    {
    }

    public District(string name, int cityId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        CityId = cityId;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CityId { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();
}