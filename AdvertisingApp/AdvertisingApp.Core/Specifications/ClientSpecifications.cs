using AdvertisingApp.Core.Entities;
using Ardalis.Specification;

namespace AdvertisingApp.Core.Specifications;

public class ClientByIdSpec : Specification<Client>, ISingleResultSpecification<Client>
{
    public ClientByIdSpec(int id)
    {
        Query.Where(c => c.Id == id);
    }
}

public class ClientWithContractsSpec : Specification<Client>, ISingleResultSpecification<Client>
{
    public ClientWithContractsSpec(int id)
    {
        Query.Where(c => c.Id == id)
            .Include(c => c.Contracts);
    }
}

public class ClientListSpec : Specification<Client>
{
    public ClientListSpec()
    {
        Query.OrderBy(c => c.Name);
    }
}