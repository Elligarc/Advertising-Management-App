using AdvertisingApp.Core.Interfaces;

namespace AdvertisingApp.Core.Entities;

public class ContractItemSchedule : IAggregateRoot
{
    private ContractItemSchedule()
    {
    }

    public ContractItemSchedule(int contractItemId, int[] daysOfWeek, int[] hoursInDay)
    {
        if (daysOfWeek == null || daysOfWeek.Length == 0)
            throw new ArgumentException("Days of week are required", nameof(daysOfWeek));

        if (hoursInDay == null || hoursInDay.Length == 0)
            throw new ArgumentException("Hours in day are required", nameof(hoursInDay));

        ContractItemId = contractItemId;
        DaysOfWeek = daysOfWeek;
        HoursInDay = hoursInDay;
    }

    public int Id { get; set; }

    public int ContractItemId { get; set; }

    // Массив дней недели (0-6, где 0 - понедельник)
    public int[] DaysOfWeek { get; set; } = new int[7];

    // Массив часов дня (0-23)
    public int[] HoursInDay { get; set; } = new int[24];

    public virtual ContractItem? ContractItem { get; set; }
}