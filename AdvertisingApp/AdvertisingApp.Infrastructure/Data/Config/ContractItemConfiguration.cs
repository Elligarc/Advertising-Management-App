using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ContractItemConfiguration : IEntityTypeConfiguration<ContractItem>
{
    public void Configure(EntityTypeBuilder<ContractItem> builder)
    {
        builder.ToTable("ContractItem");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ci => ci.SurfaceId)
            .IsRequired();

        builder.Property(ci => ci.ContractId)
            .IsRequired();

        builder.Property(ci => ci.StartDate)
            .IsRequired();

        builder.Property(ci => ci.EndDate)
            .IsRequired();

        builder.Property(ci => ci.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(ci => ci.PriceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ci => ci.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(ci => ci.Surface)
            .WithMany()
            .HasForeignKey(ci => ci.SurfaceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.Contract)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Schedule)
            .WithOne(s => s.ContractItem)
            .HasForeignKey<ContractItemSchedule>(s => s.ContractItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Registry)
            .WithOne(r => r.ContractItem)
            .HasForeignKey<ContractRegistry>(r => r.ContractItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}