using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class SurfaceStatusConfiguration : IEntityTypeConfiguration<SurfaceStatus>
{
    public void Configure(EntityTypeBuilder<SurfaceStatus> builder)
    {
        builder.ToTable("SurfaceStatus");

        builder.HasKey(ss => ss.Id);

        builder.Property(ss => ss.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ss => ss.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ss => ss.DateFrom)
            .IsRequired();

        builder.Property(ss => ss.SurfaceId)
            .IsRequired();
    }
}