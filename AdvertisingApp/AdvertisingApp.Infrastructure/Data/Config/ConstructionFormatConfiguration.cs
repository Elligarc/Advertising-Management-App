using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ConstructionFormatConfiguration : IEntityTypeConfiguration<ConstructionFormat>
{
    public void Configure(EntityTypeBuilder<ConstructionFormat> builder)
    {
        builder.ToTable("ConstructionFormat");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.ConstructionType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(f => f.Constructions)
            .WithOne(c => c.Format)
            .HasForeignKey(c => c.FormatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}