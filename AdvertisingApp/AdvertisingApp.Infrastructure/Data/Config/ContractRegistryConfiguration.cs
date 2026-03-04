using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ContractRegistryConfiguration : IEntityTypeConfiguration<ContractRegistry>
{
    public void Configure(EntityTypeBuilder<ContractRegistry> builder)
    {
        builder.ToTable("ContractRegistry");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.ContractItemId)
            .IsRequired();

        builder.Property(r => r.ContractId)
            .IsRequired();

        builder.Property(r => r.ContractStartDate)
            .IsRequired();

        builder.Property(r => r.ContractEndDate)
            .IsRequired();

        builder.Property(r => r.ContractTotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.ContractStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.ItemStartDate)
            .IsRequired();

        builder.Property(r => r.ItemEndDate)
            .IsRequired();

        builder.Property(r => r.ItemPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.ItemPriceType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.ItemTotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.SurfaceId)
            .IsRequired();

        builder.Property(r => r.ClientId)
            .IsRequired();

        builder.Property(r => r.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.ClientPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(r => r.ContractItem)
            .WithOne(ci => ci.Registry)
            .HasForeignKey<ContractRegistry>(r => r.ContractItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // Индекс для быстрого поиска
        builder.HasIndex(r => r.SurfaceId);
        builder.HasIndex(r => r.ClientId);
        builder.HasIndex(r => r.ContractStatus);
        builder.HasIndex(r => r.ItemStartDate);
        builder.HasIndex(r => r.ItemEndDate);
    }
}