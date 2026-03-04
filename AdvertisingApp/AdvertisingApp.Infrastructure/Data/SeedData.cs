using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingApp.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (await context.Cities.AnyAsync()) return;

        // 1. Города
        var cities = new List<City>
        {
            new("Москва"),
            new("Санкт-Петербург"),
            new("Новосибирск"),
            new("Екатеринбург")
        };
        context.Cities.AddRange(cities);
        await context.SaveChangesAsync();

        // 2. Районы (по 2-3 на каждый город)
        var districts = new List<District>
        {
            // Москва
            new("Центральный", cities[0].Id),
            new("Северный", cities[0].Id),
            new("Южный", cities[0].Id),
            // Санкт-Петербург
            new("Невский", cities[1].Id),
            new("Василеостровский", cities[1].Id),
            // Новосибирск
            new("Центральный", cities[2].Id),
            new("Октябрьский", cities[2].Id),
            // Екатеринбург
            new("Ленинский", cities[3].Id),
            new("Верх-Исетский", cities[3].Id)
        };
        context.Districts.AddRange(districts);
        await context.SaveChangesAsync();

        // 3. Форматы конструкций (все типы ConstructionType)
        var formats = new List<ConstructionFormat>
        {
            new("Билборд 3x6", ConstructionType.Billboard),
            new("Билборд 4x8", ConstructionType.Billboard),
            new("Плакат A1", ConstructionType.Poster),
            new("Плакат A0", ConstructionType.Poster),
            new("Ситилайт", ConstructionType.CityLight),
            new("Ситиформат", ConstructionType.CityLight),
            new("Видеоборд 9x16", ConstructionType.Videoboard),
            new("Видеоэкран", ConstructionType.Videoboard)
        };
        context.ConstructionFormats.AddRange(formats);
        await context.SaveChangesAsync();

        // 4. Конструкции (смешанные форматы по городам)
        var constructions = new List<Construction>
        {
            // Москва - Центральный
            new("ул. Арбат, 15", districts[0].Id, formats[0].Id), // Билборд 3x6
            new("ул. Тверская, 22", districts[0].Id, formats[4].Id), // Ситилайт
            new("Манежная пл., 1", districts[0].Id, formats[6].Id), // Видеоборд
            // Москва - Северный
            new("пр. Ленинградский, 45", districts[1].Id, formats[1].Id), // Билборд 4x8
            new("ул. Дмитровское, 78", districts[1].Id, formats[2].Id), // Плакат A1
            // Москва - Южный
            new("ул. Варшавское ш., 95", districts[2].Id, formats[7].Id), // Видеоэкран
            new("Каширское ш., 23", districts[2].Id, formats[0].Id), // Билборд 3x6
            // Санкт-Петербург - Невский
            new("Невский пр., 50", districts[3].Id, formats[3].Id), // Плакат A0
            new("Невский пр., 120", districts[3].Id, formats[5].Id), // Ситиформат
            // Санкт-Петербург - Василеостровский
            new("Большой пр. В.О., 32", districts[4].Id, formats[4].Id), // Ситилайт
            new("6-я линия В.О., 15", districts[4].Id, formats[0].Id), // Билборд 3x6
            // Новосибирск - Центральный
            new("ул. Ленина, 10", districts[5].Id, formats[0].Id), // Билборд 3x6
            new("Красный пр., 25", districts[5].Id, formats[2].Id), // Плакат A1
            // Новосибирск - Октябрьский
            new("ул. Кирова, 46", districts[6].Id, formats[4].Id), // Ситилайт
            new("ул. Большевистская, 131", districts[6].Id, formats[1].Id), // Билборд 4x8
            // Екатеринбург - Ленинский
            new("ул. Ленина, 25", districts[7].Id, formats[3].Id), // Плакат A0
            new("ул. 8 Марта, 55", districts[7].Id, formats[6].Id), // Видеоборд
            // Екатеринбург - Верх-Исетский
            new("ул. Металлургов, 75", districts[8].Id, formats[5].Id), // Ситиформат
            new("ул. Черепанова, 28", districts[8].Id, formats[0].Id) // Билборд 3x6
        };
        context.Constructions.AddRange(constructions);
        await context.SaveChangesAsync();

        // 5. Поверхности (все варианты Side, SurfaceType)
        var surfaces = new List<Surface>
        {
            // Москва - ул. Арбат (констр. 0) - Билборд
            new(Side.A, SurfaceType.Regular, constructions[0].Id),
            new(Side.B, SurfaceType.Regular, constructions[0].Id),
            // Москва - ул. Тверская (констр. 1) - Ситилайт
            new(Side.A, SurfaceType.Digital, constructions[1].Id, 60, 10), // 60 сек цикл, 10 сек слот
            // Москва - Манежная пл. (констр. 2) - Видеоборд
            new(Side.A, SurfaceType.Digital, constructions[2].Id, 120, 15), // 120 сек цикл, 15 сек слот
            new(Side.B, SurfaceType.Digital, constructions[2].Id, 90, 10), // 90 сек цикл, 10 сек слот
            // Москва - пр. Ленинградский (констр. 3) - Билборд 4x8
            new(Side.A, SurfaceType.Regular, constructions[3].Id),
            // Москва - ул. Дмитровское (констр. 4) - Плакат
            new(Side.A, SurfaceType.Regular, constructions[4].Id),
            new(Side.B, SurfaceType.Regular, constructions[4].Id),
            new(Side.C, SurfaceType.Regular, constructions[4].Id),
            // Москва - Варшавское ш. (констр. 5) - Видеоэкран
            new(Side.A, SurfaceType.Digital, constructions[5].Id, 180, 20), // 180 сек цикл, 20 сек слот
            // Москва - Каширское ш. (констр. 6) - Билборд
            new(Side.A, SurfaceType.Regular, constructions[6].Id),
            // СПБ - Невский пр. 50 (констр. 7) - Плакат A0
            new(Side.A, SurfaceType.Regular, constructions[7].Id),
            new(Side.B, SurfaceType.Regular, constructions[7].Id),
            // СПБ - Невский пр. 120 (констр. 8) - Ситиформат
            new(Side.A, SurfaceType.Digital, constructions[8].Id, 30, 5), // 30 сек цикл, 5 сек слот
            // СПБ - Большой пр. В.О. (констр. 9) - Ситилайт
            new(Side.A, SurfaceType.Digital, constructions[9].Id, 60, 10),
            // СПБ - 6-я линия В.О. (констр. 10) - Билборд
            new(Side.A, SurfaceType.Regular, constructions[10].Id),
            // Новосибирск - ул. Ленина (констр. 11) - Билборд
            new(Side.A, SurfaceType.Regular, constructions[11].Id),
            new(Side.B, SurfaceType.Regular, constructions[11].Id),
            // Новосибирск - Красный пр. (констр. 12) - Плакат
            new(Side.A, SurfaceType.Regular, constructions[12].Id),
            // Новосибирск - ул. Кирова (констр. 13) - Ситилайт
            new(Side.A, SurfaceType.Digital, constructions[13].Id, 60, 10),
            // Новосибирск - ул. Большевистская (констр. 14) - Билборд 4x8
            new(Side.A, SurfaceType.Regular, constructions[14].Id),
            // Екатеринбург - ул. Ленина (констр. 15) - Плакат A0
            new(Side.A, SurfaceType.Regular, constructions[15].Id),
            new(Side.B, SurfaceType.Regular, constructions[15].Id),
            // Екатеринбург - ул. 8 Марта (констр. 16) - Видеоборд
            new(Side.A, SurfaceType.Digital, constructions[16].Id, 120, 20),
            // Екатеринбург - ул. Металлургов (констр. 17) - Ситиформат
            new(Side.A, SurfaceType.Digital, constructions[17].Id, 45, 15),
            // Екатеринбург - ул. Черепанова (констр. 18) - Билборд
            new(Side.A, SurfaceType.Regular, constructions[18].Id),
            new(Side.B, SurfaceType.Regular, constructions[18].Id),
            new(Side.C, SurfaceType.Regular, constructions[18].Id)
        };
        context.Surfaces.AddRange(surfaces);
        await context.SaveChangesAsync();

        // 6. Статусы поверхностей (все варианты Status)
        var statuses = new List<SurfaceStatus>
        {
            // Активные (Created)
            new(Status.Created, DateTime.UtcNow, surfaces[0].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[1].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[2].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[3].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[4].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[5].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[6].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[7].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[8].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[9].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[10].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[11].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[12].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[13].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[14].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[15].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[16].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[17].Id),
            new(Status.Created, DateTime.UtcNow, surfaces[18].Id),
            // В ремонте (UnderRepair)
            new(Status.UnderRepair, DateTime.UtcNow.AddDays(-5), surfaces[19].Id),
            new(Status.UnderRepair, DateTime.UtcNow.AddDays(-2), surfaces[20].Id),
            // Выведено из эксплуатации (Decommissioned)
            new(Status.Decommissioned, DateTime.UtcNow.AddMonths(-3), surfaces[21].Id),
            new(Status.Decommissioned, DateTime.UtcNow.AddMonths(-1), surfaces[22].Id)
        };
        context.SurfaceStatuses.AddRange(statuses);
        await context.SaveChangesAsync();

        // 7. Прайс-лист (все варианты PriceType)
        var priceLists = new List<PriceList>
        {
            // Для Regular поверхностей - PerMonth
            new(PriceType.PerMonth, 15000m, DateTime.UtcNow, surfaces[0].Id),
            new(PriceType.PerMonth, 18000m, DateTime.UtcNow, surfaces[1].Id),
            new(PriceType.PerMonth, 12000m, DateTime.UtcNow, surfaces[5].Id),
            new(PriceType.PerMonth, 8000m, DateTime.UtcNow, surfaces[6].Id),
            new(PriceType.PerMonth, 9000m, DateTime.UtcNow, surfaces[7].Id),
            new(PriceType.PerMonth, 10000m, DateTime.UtcNow, surfaces[8].Id),
            new(PriceType.PerMonth, 25000m, DateTime.UtcNow, surfaces[9].Id),
            new(PriceType.PerMonth, 11000m, DateTime.UtcNow, surfaces[10].Id),
            new(PriceType.PerMonth, 13000m, DateTime.UtcNow, surfaces[11].Id),
            new(PriceType.PerMonth, 14000m, DateTime.UtcNow, surfaces[12].Id),
            new(PriceType.PerMonth, 7000m, DateTime.UtcNow, surfaces[13].Id),
            new(PriceType.PerMonth, 16000m, DateTime.UtcNow, surfaces[14].Id),
            new(PriceType.PerMonth, 8500m, DateTime.UtcNow, surfaces[15].Id),
            new(PriceType.PerMonth, 9000m, DateTime.UtcNow, surfaces[16].Id),
            new(PriceType.PerMonth, 22000m, DateTime.UtcNow, surfaces[17].Id),
            new(PriceType.PerMonth, 9500m, DateTime.UtcNow, surfaces[18].Id),
            // Для Digital поверхностей - PerShow
            new(PriceType.PerShow, 50m, DateTime.UtcNow, surfaces[2].Id),
            new(PriceType.PerShow, 80m, DateTime.UtcNow, surfaces[3].Id),
            new(PriceType.PerShow, 60m, DateTime.UtcNow, surfaces[4].Id),
            new(PriceType.PerShow, 100m, DateTime.UtcNow, surfaces[9].Id),
            new(PriceType.PerShow, 30m, DateTime.UtcNow, surfaces[10].Id),
            new(PriceType.PerShow, 55m, DateTime.UtcNow, surfaces[13].Id),
            new(PriceType.PerShow, 75m, DateTime.UtcNow, surfaces[16].Id),
            new(PriceType.PerShow, 45m, DateTime.UtcNow, surfaces[17].Id)
        };
        context.PriceLists.AddRange(priceLists);
        await context.SaveChangesAsync();

        // 8. Клиенты
        var clients = new List<Client>
        {
            new("ООО \"Реклама Плюс\"", "+7 (495) 123-45-67"),
            new("ИП Иванов А.А.", "+7 (812) 987-65-43"),
            new("ОАО \"Наружка\"", "+7 (383) 222-33-44"),
            new("ООО \"Медиа Арт\"", "+7 (343) 555-66-77"),
            new("ЗАО \"Билборд Групп\"", "+7 (495) 111-22-33"),
            new("ИП Сидоров В.П.", "+7 (383) 444-55-66")
        };
        context.Clients.AddRange(clients);
        await context.SaveChangesAsync();

        // 9. Контракты (все статусы ContractStatus)
        var contracts = new List<Contract>
        {
            // Активные контракты
            new(clients[0].Id, 15000m) { StartDate = DateTime.UtcNow.AddDays(-10), EndDate = DateTime.UtcNow.AddMonths(1) },
            new(clients[1].Id, 30000m) { StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddMonths(2) },
            new(clients[2].Id, 45000m) { StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddMonths(3) },
            // Созданные (будущие)
            new(clients[3].Id, 20000m) { StartDate = DateTime.UtcNow.AddDays(14), EndDate = DateTime.UtcNow.AddMonths(1) },
            new(clients[4].Id, 35000m) { StartDate = DateTime.UtcNow.AddDays(7), EndDate = DateTime.UtcNow.AddMonths(2) },
            // Отмененные
            new(clients[5].Id, 50000m) { StartDate = DateTime.UtcNow.AddDays(30), EndDate = DateTime.UtcNow.AddMonths(2) }
        };
        contracts[0].Status = ContractStatus.Active;
        contracts[1].Status = ContractStatus.Active;
        contracts[2].Status = ContractStatus.Active;
        contracts[5].Status = ContractStatus.Cancelled;
        context.Contracts.AddRange(contracts);
        await context.SaveChangesAsync();

        // 10. Элементы контрактов
        var contractItems = new List<ContractItem>
        {
            // Контракт 1 (активный) - ООО "Реклама Плюс"
            new(surfaces[0].Id, contracts[0].Id, contracts[0].StartDate!.Value, contracts[0].EndDate!.Value, 15000m,
                PriceType.PerMonth),
            // Контракт 2 (активный) - ИП Иванов
            new(surfaces[2].Id, contracts[1].Id, contracts[1].StartDate!.Value, contracts[1].EndDate!.Value, 18000m,
                PriceType.PerShow),
            new(surfaces[3].Id, contracts[1].Id, contracts[1].StartDate!.Value, contracts[1].EndDate!.Value, 12000m,
                PriceType.PerShow),
            // Контракт 3 (активный) - ОАО "Наружка"
            new(surfaces[5].Id, contracts[2].Id, contracts[2].StartDate!.Value, contracts[2].EndDate!.Value, 25000m,
                PriceType.PerMonth),
            new(surfaces[6].Id, contracts[2].Id, contracts[2].StartDate!.Value, contracts[2].EndDate!.Value, 10000m,
                PriceType.PerMonth),
            new(surfaces[10].Id, contracts[2].Id, contracts[2].StartDate!.Value, contracts[2].EndDate!.Value, 10000m,
                PriceType.PerMonth),
            // Контракт 4 (созданный) - ООО "Медиа Арт"
            new(surfaces[11].Id, contracts[3].Id, contracts[3].StartDate!.Value, contracts[3].EndDate!.Value, 20000m,
                PriceType.PerMonth),
            // Контракт 5 (созданный) - ЗАО "Билборд Групп"
            new(surfaces[13].Id, contracts[4].Id, contracts[4].StartDate!.Value, contracts[4].EndDate!.Value, 20000m,
                PriceType.PerShow),
            new(surfaces[16].Id, contracts[4].Id, contracts[4].StartDate!.Value, contracts[4].EndDate!.Value, 15000m,
                PriceType.PerShow),
            // Контракт 6 (отмененный) - ИП Сидоров
            new(surfaces[18].Id, contracts[5].Id, contracts[5].StartDate!.Value, contracts[5].EndDate!.Value, 50000m,
                PriceType.PerMonth)
        };
        context.ContractItems.AddRange(contractItems);
        await context.SaveChangesAsync();

        // 11. Расписания элементов контрактов
        var schedules = new List<ContractItemSchedule>
        {
            // Контракт 1 - все дни, рабочие часы
            new(contractItems[0].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 }),
            // Контракт 2 - будни, полный день
            new(contractItems[1].Id, new[] { 0, 1, 2, 3, 4 }, new[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }),
            new(contractItems[2].Id, new[] { 0, 1, 2, 3, 4 }, new[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }),
            // Контракт 3 - все дни, круглосуточно
            new(contractItems[3].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }),
            new(contractItems[4].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }),
            new(contractItems[5].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }),
            // Контракт 4 - выходные + будни, дневное время
            new(contractItems[6].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }),
            // Контракт 5 - будни, утро-день
            new(contractItems[7].Id, new[] { 0, 1, 2, 3, 4 }, new[] { 8, 9, 10, 11, 12, 13, 14 }),
            new(contractItems[8].Id, new[] { 0, 1, 2, 3, 4 }, new[] { 8, 9, 10, 11, 12, 13, 14 }),
            // Контракт 6 - все дни
            new(contractItems[9].Id, new[] { 0, 1, 2, 3, 4, 5, 6 },
                new[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 })
        };
        context.ContractItemSchedules.AddRange(schedules);
        await context.SaveChangesAsync();

        // 12. Реестры контрактов
        var registries = new List<ContractRegistry>();
        for (var i = 0; i < contractItems.Count; i++)
        {
            var contract = contracts[contractItems[i].ContractId - 1];
            var client = clients[contract.ClientId - 1];
            var surface = surfaces[contractItems[i].SurfaceId - 1];

            registries.Add(new ContractRegistry(
                contractItems[i].Id,
                contract.Id,
                contract.StartDate!.Value,
                contract.EndDate!.Value,
                contract.TotalPrice,
                contract.Status,
                contractItems[i].StartDate,
                contractItems[i].EndDate,
                contractItems[i].Price,
                contractItems[i].PriceType,
                contractItems[i].TotalPrice,
                surface.Id,
                client.Id,
                client.Name,
                client.Phone
            ));
        }

        context.ContractRegistries.AddRange(registries);
        await context.SaveChangesAsync();

        // 13. Бронирования поверхностей
        var bookings = new List<SurfaceBooking>
        {
            // Бронирования для поверхностей с активными контрактами
            new(DateTime.UtcNow.Date, 10, 1, surfaces[0].Id),
            new(DateTime.UtcNow.Date, 11, 1, surfaces[0].Id),
            new(DateTime.UtcNow.Date, 14, 1, surfaces[2].Id),
            new(DateTime.UtcNow.Date, 15, 1, surfaces[2].Id),
            new(DateTime.UtcNow.Date, 16, 1, surfaces[2].Id),
            new(DateTime.UtcNow.Date, 9, 1, surfaces[3].Id),
            new(DateTime.UtcNow.Date, 10, 1, surfaces[3].Id),
            new(DateTime.UtcNow.Date, 12, 1, surfaces[5].Id),
            new(DateTime.UtcNow.Date.AddDays(1), 9, 1, surfaces[5].Id),
            new(DateTime.UtcNow.Date.AddDays(1), 10, 1, surfaces[5].Id),
            new(DateTime.UtcNow.Date, 14, 1, surfaces[6].Id),
            new(DateTime.UtcNow.Date, 15, 1, surfaces[6].Id),
            new(DateTime.UtcNow.Date, 16, 1, surfaces[6].Id),
            new(DateTime.UtcNow.Date, 10, 1, surfaces[10].Id),
            new(DateTime.UtcNow.Date.AddDays(1), 11, 1, surfaces[10].Id),
            // Бронирования для цифровых поверхностей (множественные слоты)
            new(DateTime.UtcNow.Date, 9, 1, surfaces[2].Id),
            new(DateTime.UtcNow.Date, 10, 2, surfaces[2].Id), // 2 слота занято
            new(DateTime.UtcNow.Date, 11, 1, surfaces[2].Id),
            new(DateTime.UtcNow.Date, 9, 3, surfaces[3].Id), // 3 слота занято
            new(DateTime.UtcNow.Date, 10, 2, surfaces[3].Id),
            // Будущие бронирования
            new(DateTime.UtcNow.AddDays(7).Date, 9, 1, surfaces[11].Id),
            new(DateTime.UtcNow.AddDays(7).Date, 10, 1, surfaces[11].Id),
            new(DateTime.UtcNow.AddDays(8).Date, 14, 1, surfaces[13].Id),
            new(DateTime.UtcNow.AddDays(8).Date, 15, 1, surfaces[13].Id)
        };
        context.SurfaceBookings.AddRange(bookings);
        await context.SaveChangesAsync();
    }
}