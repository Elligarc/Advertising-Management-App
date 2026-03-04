using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class ConstructionFormat : IAggregateRoot
{
    private ConstructionFormat()
    {
    }

    public ConstructionFormat(string name, ConstructionType constructionType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        ConstructionType = constructionType;
    }

    public int Id { get; set; }

    public ConstructionType ConstructionType { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();
}