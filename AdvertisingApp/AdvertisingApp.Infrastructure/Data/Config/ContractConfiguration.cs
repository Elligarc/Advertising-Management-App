using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contract");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClientId)
            .IsRequired();

        builder.Property(c => c.StartDate)
            .IsRequired(false);

        builder.Property(c => c.EndDate)
            .IsRequired(false);

        builder.Property(c => c.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasMany(c => c.Items)
            .WithOne(i => i.Contract)
            .HasForeignKey(i => i.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}