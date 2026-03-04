using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class SurfaceConfiguration : IEntityTypeConfiguration<Surface>
{
    public void Configure(EntityTypeBuilder<Surface> builder)
    {
        builder.ToTable("Surface");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Side)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.SurfaceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.LoopDuration);

        builder.Property(s => s.SlotDuration);

        builder.Property(s => s.MaxSlots)
            .IsRequired();

        builder.Property(s => s.ConstructionId)
            .IsRequired();

        builder.HasMany(s => s.Statuses)
            .WithOne(ss => ss.Surface)
            .HasForeignKey(ss => ss.SurfaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.PriceLists)
            .WithOne(pl => pl.Surface)
            .HasForeignKey(pl => pl.SurfaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Surface)
            .HasForeignKey(b => b.SurfaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}