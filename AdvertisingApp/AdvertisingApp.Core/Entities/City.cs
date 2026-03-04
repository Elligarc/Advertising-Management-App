using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class City : IAggregateRoot
{
    private City()
    {
    }

    public City(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}