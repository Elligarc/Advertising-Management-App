using AdvertisingApp.Core.Enums;

namespace AdvertisingApp.Core.Contracts;

public class FilterSurfacesData
{
    // Фильтрация по местоположению
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }

    // Фильтрация по типу конструкции и формату
    public int? FormatId { get; set; }
    public ConstructionType? ConstructionType { get; set; }

    // Фильтрация по типу ценообразования
    public PriceType? PriceType { get; set; }

    // Для PriceType = PerShow: интервал дней и маска часов
    public DateTime? AvailableDaysFrom { get; set; }
    public DateTime? AvailableDaysTo { get; set; }
    public bool[]? AvailableHours { get; set; } // Маска 24 часа: true = час выбран

    // Для PriceType = PerMonth: список свободных месяцев
    public List<int>? AvailableMonths { get; set; }
}