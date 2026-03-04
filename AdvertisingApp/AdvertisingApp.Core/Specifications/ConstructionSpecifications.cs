using AdvertisingApp.Core.Entities;
using Ardalis.Specification;

namespace AdvertisingApp.Core.Specifications;

public class ConstructionByIdWithDetailsSpecification : Specification<Construction>,
    ISingleResultSpecification<Construction>
{
    public ConstructionByIdWithDetailsSpecification(int id)
    {
        Query.Where(c => c.Id == id)
            .Include(c => c.District)
            .ThenInclude(d => d.City)
            .Include(c => c.Format);
    }
}

public class AllConstructionsWithDetailsSpecification : Specification<Construction>
{
    public AllConstructionsWithDetailsSpecification()
    {
        Query.Include(c => c.District)
            .ThenInclude(d => d.City)
            .Include(c => c.Format);
    }
}