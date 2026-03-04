using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class Client : IAggregateRoot
{
    private Client()
    {
    }

    public Client(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        Phone = phone ?? string.Empty;
    }

    public int Id { get; set; }

    public string Name { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public void Update(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name;
        Phone = phone ?? string.Empty;
    }
}