using AdvertisingApp.Core.Entities;
using Ardalis.Specification;

namespace AdvertisingApp.Core.Specifications;

// Contract Specifications
public class ContractByIdWithItemsSpecification : Specification<Contract>, ISingleResultSpecification<Contract>
{
    public ContractByIdWithItemsSpecification(int id)
    {
        Query.Where(c => c.Id == id)
            .Include(c => c.Client)
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Surface)
            .ThenInclude(s => s!.Construction)
            .ThenInclude(c => c!.District)
            .ThenInclude(d => d!.City)
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Schedule);
    }
}

public class AllContractsSpecification : Specification<Contract>
{
    public AllContractsSpecification()
    {
        Query.Include(c => c.Client)
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Surface)
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Schedule);
    }
}

// Surface Specifications
public class SurfaceByConstructionSpecification : Specification<Surface>
{
    public SurfaceByConstructionSpecification(int constructionId)
    {
        Query.Where(s => s.ConstructionId == constructionId)
            .Include(s => s.Construction)
            .ThenInclude(c => c!.District)
            .ThenInclude(c => c!.City)
            .Include(s => s.Construction)
            .ThenInclude(c => c!.Format)
            .Include(s => s.PriceLists)
            .Include(s => s.Statuses);
    }
}

public class SurfaceByIdWithDetailsSpecification : Specification<Surface>, ISingleResultSpecification<Surface>
{
    public SurfaceByIdWithDetailsSpecification(int id)
    {
        Query.Where(s => s.Id == id)
            .Include(s => s.Construction)
            .ThenInclude(c => c!.District)
            .ThenInclude(c => c!.City)
            .Include(s => s.Construction)
            .ThenInclude(c => c!.Format)
            .Include(s => s.PriceLists)
            .Include(s => s.Bookings)
            .Include(s => s.Statuses);
    }
}

public class AllSurfacesWithDetailsSpecification : Specification<Surface>
{
    public AllSurfacesWithDetailsSpecification()
    {
        Query.Include(s => s.Construction)
            .ThenInclude(c => c!.District)
            .ThenInclude(d => d!.City)
            .Include(s => s.Construction)
            .ThenInclude(c => c!.Format)
            .Include(s => s.PriceLists)
            .Include(s => s.Bookings)
            .Include(s => s.Statuses);
    }
}

// SurfaceBooking Specifications
public class SurfaceBookingByDateHourSpecification : Specification<SurfaceBooking>,
    ISingleResultSpecification<SurfaceBooking>
{
    public SurfaceBookingByDateHourSpecification(int surfaceId, DateTime date, int hour)
    {
        Query.Where(b => b.SurfaceId == surfaceId && b.Date.Date == date.Date && b.Hour == hour);
    }
}