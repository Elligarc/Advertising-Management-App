using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using Ardalis.Specification.EntityFrameworkCore;

namespace AdvertisingApp.Infrastructure.Repositories;

public class EfRepository<T>(AppDbContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>, IReadRepository<T>
    where T : class, IAggregateRoot
{
    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}