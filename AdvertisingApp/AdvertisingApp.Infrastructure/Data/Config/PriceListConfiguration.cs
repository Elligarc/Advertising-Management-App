using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class PriceListConfiguration : IEntityTypeConfiguration<PriceList>
{
    public void Configure(EntityTypeBuilder<PriceList> builder)
    {
        builder.ToTable("PriceList");

        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.Id)
            .ValueGeneratedOnAdd();

        builder.Property(pl => pl.PriceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(pl => pl.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pl => pl.DateFrom)
            .IsRequired();

        builder.Property(pl => pl.SurfaceId)
            .IsRequired();
    }
}