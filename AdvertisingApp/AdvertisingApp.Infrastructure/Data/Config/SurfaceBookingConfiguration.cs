using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class SurfaceBookingConfiguration : IEntityTypeConfiguration<SurfaceBooking>
{
    public void Configure(EntityTypeBuilder<SurfaceBooking> builder)
    {
        builder.ToTable("SurfaceBooking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Date)
            .IsRequired();

        builder.Property(b => b.Hour)
            .IsRequired();

        builder.Property(b => b.SlotsOccupied)
            .IsRequired();

        builder.Property(b => b.SurfaceId)
            .IsRequired();
    }
}