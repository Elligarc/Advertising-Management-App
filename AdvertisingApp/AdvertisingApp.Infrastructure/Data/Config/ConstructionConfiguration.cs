using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ConstructionConfiguration : IEntityTypeConfiguration<Construction>
{
    public void Configure(EntityTypeBuilder<Construction> builder)
    {
        builder.ToTable("Construction");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.DistrictId)
            .IsRequired();

        builder.Property(c => c.FormatId)
            .IsRequired();

        builder.HasMany(c => c.Surfaces)
            .WithOne(s => s.Construction)
            .HasForeignKey(s => s.ConstructionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}