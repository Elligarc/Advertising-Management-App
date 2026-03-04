using AdvertisingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingApp.Infrastructure.Data.Config;

public class ContractItemScheduleConfiguration : IEntityTypeConfiguration<ContractItemSchedule>
{
    public void Configure(EntityTypeBuilder<ContractItemSchedule> builder)
    {
        builder.ToTable("ContractItemSchedule");

        builder.HasKey(s => s.ContractItemId);

        builder.Property(s => s.ContractItemId)
            .ValueGeneratedNever();

        builder.Property(s => s.DaysOfWeek)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
            );

        builder.Property(s => s.HoursInDay)
            .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()
            );

        builder.HasOne(s => s.ContractItem)
            .WithOne(ci => ci.Schedule)
            .HasForeignKey<ContractItemSchedule>(s => s.ContractItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}