using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Specifications;

namespace AdvertisingApp.Core.Services;

public class ContractService : IContractService
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IRepository<Contract> _contractRepository;
    private readonly IRepository<SurfaceBooking> _surfaceBookingRepository;
    private readonly IRepository<Surface> _surfaceRepository;

    public ContractService(
        IRepository<Contract> contractRepository,
        IRepository<Surface> surfaceRepository,
        IRepository<SurfaceBooking> surfaceBookingRepository,
        IRepository<Client> clientRepository)
    {
        _contractRepository = contractRepository;
        _surfaceRepository = surfaceRepository;
        _surfaceBookingRepository = surfaceBookingRepository;
        _clientRepository = clientRepository;
    }

    public async Task<Contract> CreateAsync(CreateContractData data)
    {
        var client = await _clientRepository.GetByIdAsync(data.ClientId);
        if (client == null)
            throw new KeyNotFoundException($"Client with id {data.ClientId} not found");

        var contract = new Contract(data.ClientId, 0);

        await _contractRepository.AddAsync(contract);
        await _contractRepository.SaveChangesAsync();

        return contract;
    }

    public async Task<ContractItem> AddItemAsync(int contractId, CreateContractItemData data)
    {
        var contract = await _contractRepository.FirstOrDefaultAsync(
            new ContractByIdWithItemsSpecification(contractId));

        if (contract == null)
            throw new KeyNotFoundException($"Contract with id {contractId} not found");

        if (contract.Status != ContractStatus.Created)
            throw new InvalidOperationException("Contract can only be modified when status is Created");

        var surface = await _surfaceRepository.FirstOrDefaultAsync(
            new SurfaceByIdWithDetailsSpecification(data.SurfaceId));

        if (surface == null)
            throw new KeyNotFoundException($"Surface with id {data.SurfaceId} not found");

        // Проверка, что поверхность ещё не добавлена в этот договор
        if (contract.Items.Any(i => i.SurfaceId == data.SurfaceId))
            throw new InvalidOperationException(
                $"Surface with id {data.SurfaceId} is already added to this contract");

        await CheckSurfaceAvailabilityAsync(surface, data);

        // Расчёт итоговой цены
        var totalPrice = CalculateTotalPrice(surface, data);

        var contractItem = new ContractItem(
            data.SurfaceId,
            contractId,
            data.StartDate,
            data.EndDate,
            data.Price,
            data.PriceType)
        {
            TotalPrice = totalPrice
        };

        // Если Digital поверхность - создаём расписание
        if (surface.SurfaceType == SurfaceType.Digital)
        {
            var schedule = new ContractItemSchedule(
                0, // ContractItemId will be set after adding
                data.DaysOfWeek ?? Array.Empty<int>(),
                data.HoursInDay ?? Array.Empty<int>());

            contractItem.Schedule = schedule;
        }

        contract.Items.Add(contractItem);

        // Обновляем общую сумму договора
        contract.TotalPrice += totalPrice;

        // Вычисляем StartDate и EndDate контракта на основе минимальной и максимальной дат из Items
        UpdateContractDates(contract);

        // Создаём SurfaceBooking записи
        await CreateSurfaceBookingsAsync(surface, contractItem, data);

        await _contractRepository.SaveChangesAsync();

        return contractItem;
    }

    public async Task<Contract> UpdateAsync(int id, UpdateContractData data)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        if (contract == null)
            throw new KeyNotFoundException($"Contract with id {id} not found");

        if (contract.Status != ContractStatus.Created)
            throw new InvalidOperationException("Contract can only be modified when status is Created");

        if (data.Status.HasValue) contract.Status = data.Status.Value;

        await _contractRepository.UpdateAsync(contract);
        await _contractRepository.SaveChangesAsync();

        return contract;
    }

    public async Task<Contract?> GetByIdAsync(int id)
    {
        return await _contractRepository.FirstOrDefaultAsync(
            new ContractByIdWithItemsSpecification(id));
    }

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        return await _contractRepository.ListAsync(new AllContractsSpecification());
    }

    private async Task CheckSurfaceAvailabilityAsync(Surface surface, CreateContractItemData data)
    {
        var daysOfWeek = data.DaysOfWeek ?? Enumerable.Range(0, 7).ToArray();
        var hoursInDay = data.HoursInDay ?? Enumerable.Range(0, 24).ToArray();

        for (var date = data.StartDate.Date; date <= data.EndDate.Date; date = date.AddDays(1))
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var adjustedDayOfWeek = dayOfWeek == 0 ? 6 : dayOfWeek - 1;

            if (!daysOfWeek.Contains(adjustedDayOfWeek))
                continue;

            foreach (var hour in hoursInDay)
            {
                var existingBooking = await _surfaceBookingRepository.FirstOrDefaultAsync(
                    new SurfaceBookingByDateHourSpecification(surface.Id, date, hour));

                if (existingBooking != null && existingBooking.SlotsOccupied >= surface.MaxSlots)
                    throw new InvalidOperationException(
                        $"Surface is not available on {date:yyyy-MM-dd} at {hour}:00. " +
                        $"Occupied slots: {existingBooking.SlotsOccupied}, Max slots: {surface.MaxSlots}");
            }
        }
    }

    private decimal CalculateTotalPrice(Surface surface, CreateContractItemData data)
    {
        if (data.PriceType == PriceType.PerMonth) return CalculateMonthlyPrice(data);

        return CalculateShowPrice(surface, data);
    }

    private decimal CalculateMonthlyPrice(CreateContractItemData data)
    {
        var months = (data.EndDate.Year - data.StartDate.Year) * 12 + data.EndDate.Month - data.StartDate.Month;

        return months * data.Price;
    }

    private decimal CalculateShowPrice(Surface surface, CreateContractItemData data)
    {
        if (!surface.SlotDuration.HasValue || surface.SlotDuration.Value == 0)
            throw new InvalidOperationException("SlotDuration is required for PerShow pricing");

        var secondsInHour = 3600;
        var cyclesPerHour = secondsInHour / surface.SlotDuration.Value;

        var daysOfWeek = data.DaysOfWeek ?? Enumerable.Range(0, 7).ToArray();
        var hoursInDay = data.HoursInDay ?? Enumerable.Range(0, 24).ToArray();

        var hoursCount = hoursInDay.Length;
        var daysCount = CalculateDaysCountInRange(data.StartDate, data.EndDate, daysOfWeek);

        return cyclesPerHour * hoursCount * daysCount * data.Price;
    }

    private int CalculateDaysCountInRange(DateTime startDate, DateTime endDate, int[] daysOfWeek)
    {
        var count = 0;

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var dayOfWeek = (int)date.DayOfWeek;
            // Convert from .NET DayOfWeek (0=Sunday) to our format (0=Monday)
            var adjustedDayOfWeek = dayOfWeek == 0 ? 6 : dayOfWeek - 1;

            if (daysOfWeek.Contains(adjustedDayOfWeek)) count++;
        }

        return count;
    }

    private async Task CreateSurfaceBookingsAsync(
        Surface surface,
        ContractItem contractItem,
        CreateContractItemData data)
    {
        var daysOfWeek = data.DaysOfWeek ?? Enumerable.Range(0, 7).ToArray();
        var hoursInDay = data.HoursInDay ?? Enumerable.Range(0, 24).ToArray();

        for (var date = data.StartDate.Date; date <= data.EndDate.Date; date = date.AddDays(1))
        {
            var dayOfWeek = (int)date.DayOfWeek;
            var adjustedDayOfWeek = dayOfWeek == 0 ? 6 : dayOfWeek - 1;

            if (!daysOfWeek.Contains(adjustedDayOfWeek))
                continue;

            foreach (var hour in hoursInDay)
            {
                var existingBooking = await _surfaceBookingRepository.FirstOrDefaultAsync(
                    new SurfaceBookingByDateHourSpecification(surface.Id, date, hour));

                if (existingBooking != null)
                {
                    existingBooking.SlotsOccupied += 1;
                }
                else
                {
                    var booking = new SurfaceBooking(date, hour, 1, surface.Id);
                    await _surfaceBookingRepository.AddAsync(booking);
                }
            }
        }
    }

    private void UpdateContractDates(Contract contract)
    {
        if (!contract.Items.Any())
        {
            contract.StartDate = null;
            contract.EndDate = null;
            return;
        }

        contract.StartDate = contract.Items.Min(i => i.StartDate);
        contract.EndDate = contract.Items.Max(i => i.EndDate);
    }
}
