using AdvertisingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingApp.Tests.Helpers;

public static class TestDbContextHelper
{
    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}