using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<City> Cities => Set<City>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<ConstructionFormat> ConstructionFormats => Set<ConstructionFormat>();
    public DbSet<Construction> Constructions => Set<Construction>();
    public DbSet<Surface> Surfaces => Set<Surface>();
    public DbSet<SurfaceStatus> SurfaceStatuses => Set<SurfaceStatus>();
    public DbSet<PriceList> PriceLists => Set<PriceList>();
    public DbSet<SurfaceBooking> SurfaceBookings => Set<SurfaceBooking>();

    // Sales & Contracts
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractItem> ContractItems => Set<ContractItem>();
    public DbSet<ContractItemSchedule> ContractItemSchedules => Set<ContractItemSchedule>();
    public DbSet<ContractRegistry> ContractRegistries => Set<ContractRegistry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}