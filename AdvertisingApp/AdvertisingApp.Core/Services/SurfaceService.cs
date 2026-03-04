using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Specifications;

namespace AdvertisingApp.Core.Services;

public class SurfaceService : ISurfaceService
{
    private readonly IRepository<Construction> _constructionRepository;
    private readonly IRepository<PriceList> _priceListRepository;
    private readonly IRepository<SurfaceBooking> _surfaceBookingRepository;
    private readonly IRepository<Surface> _surfaceRepository;
    private readonly IRepository<SurfaceStatus> _surfaceStatusRepository;

    public SurfaceService(
        IRepository<Surface> surfaceRepository,
        IRepository<Construction> constructionRepository,
        IRepository<PriceList> priceListRepository,
        IRepository<SurfaceStatus> surfaceStatusRepository,
        IRepository<SurfaceBooking> surfaceBookingRepository)
    {
        _surfaceRepository = surfaceRepository;
        _constructionRepository = constructionRepository;
        _priceListRepository = priceListRepository;
        _surfaceStatusRepository = surfaceStatusRepository;
        _surfaceBookingRepository = surfaceBookingRepository;
    }

    public async Task<IEnumerable<Surface>> GetByConstructionIdAsync(int constructionId)
    {
        return await _surfaceRepository.ListAsync(new SurfaceByConstructionSpecification(constructionId));
    }

    public async Task<Surface?> GetByIdAsync(int id)
    {
        return await _surfaceRepository.FirstOrDefaultAsync(new SurfaceByIdWithDetailsSpecification(id));
    }

    public async Task<IEnumerable<Surface>> GetFilteredAsync(FilterSurfacesData filter)
    {
        var surfaces = await _surfaceRepository.ListAsync(new AllSurfacesWithDetailsSpecification());
        var filteredSurfaces = surfaces.AsEnumerable();

        if (filter.CityId.HasValue)
            filteredSurfaces = filteredSurfaces.Where(s =>
                s.Construction?.District?.CityId == filter.CityId.Value);

        if (filter.DistrictId.HasValue)
            filteredSurfaces = filteredSurfaces.Where(s =>
                s.Construction?.DistrictId == filter.DistrictId.Value);

        if (filter.FormatId.HasValue)
            filteredSurfaces = filteredSurfaces.Where(s =>
                s.Construction?.FormatId == filter.FormatId.Value);

        if (filter.ConstructionType.HasValue)
            filteredSurfaces = filteredSurfaces.Where(s =>
                s.Construction?.Format?.ConstructionType == filter.ConstructionType.Value);

        if (filter.PriceType.HasValue)
        {
            var currentDate = DateTime.UtcNow;
            filteredSurfaces = filteredSurfaces.Where(s =>
                s.PriceLists != null &&
                s.PriceLists.Any(p => p.DateFrom <= currentDate) &&
                s.PriceLists.OrderByDescending(p => p.DateFrom).First(p => p.DateFrom <= currentDate).PriceType ==
                filter.PriceType.Value);
        }

        if (filter.PriceType == PriceType.PerShow)
            filteredSurfaces = filteredSurfaces.Where(s => IsSurfaceAvailableForPerShow(s, filter));

        if (filter.PriceType == PriceType.PerMonth)
            filteredSurfaces = filteredSurfaces.Where(s => IsSurfaceAvailableForPerMonth(s, filter));

        return filteredSurfaces;
    }

    public async Task<Surface> CreateAsync(CreateSurfaceData data)
    {
        var construction = await _constructionRepository.GetByIdAsync(data.ConstructionId);
        if (construction == null)
            throw new KeyNotFoundException($"Construction with id {data.ConstructionId} not found");

        var existingSurfaces =
            await _surfaceRepository.ListAsync(new SurfaceByConstructionSpecification(data.ConstructionId));
        if (existingSurfaces.Any(s => s.Side == data.Side))
            throw new InvalidOperationException($"Surface with side {data.Side} already exists for this construction");

        if (existingSurfaces.Count() >= 3)
            throw new InvalidOperationException("Maximum of 3 surfaces per construction is allowed");

        if (data.SurfaceType == SurfaceType.Regular)
        {
            data.LoopDuration = null;
            data.SlotDuration = null;
        }
        else if (data.SurfaceType == SurfaceType.Digital)
        {
            if (!data.LoopDuration.HasValue || !data.SlotDuration.HasValue)
                throw new ArgumentException("LoopDuration and SlotDuration are required for Digital surface type");
        }

        var surface = new Surface(
            data.Side,
            data.SurfaceType,
            data.ConstructionId,
            data.LoopDuration,
            data.SlotDuration);

        await _surfaceRepository.AddAsync(surface);
        await _surfaceRepository.SaveChangesAsync();

        var priceList = new PriceList(data.PriceType, data.InitialPrice, DateTime.UtcNow, surface.Id);
        await _priceListRepository.AddAsync(priceList);

        var surfaceStatus = new SurfaceStatus(Status.Created, DateTime.UtcNow, surface.Id);
        await _surfaceStatusRepository.AddAsync(surfaceStatus);

        await _surfaceRepository.SaveChangesAsync();

        return surface;
    }

    public async Task<Surface> UpdateAsync(int id, UpdateSurfaceData data)
    {
        var surface = await _surfaceRepository.GetByIdAsync(id);
        if (surface == null)
            throw new KeyNotFoundException($"Surface with id {id} not found");

        if (data.LoopDuration.HasValue)
            surface.LoopDuration = data.LoopDuration;
        if (data.SlotDuration.HasValue)
            surface.SlotDuration = data.SlotDuration;

        surface.CalculateMaxSlots();

        await _surfaceRepository.UpdateAsync(surface);
        return surface;
    }

    public async Task<Surface> UpdatePriceAsync(int id, UpdateSurfacePriceData data)
    {
        var surface = await _surfaceRepository.GetByIdAsync(id);
        if (surface == null)
            throw new KeyNotFoundException($"Surface with id {id} not found");

        var dateFrom = data.DateFrom ?? DateTime.UtcNow.Date;

        var existingPrice = surface.PriceLists?
            .OrderByDescending(p => p.DateFrom)
            .FirstOrDefault(p => p.DateFrom.Date == dateFrom.Date);

        if (existingPrice != null)
        {
            existingPrice.Price = data.Price;
            existingPrice.PriceType = data.PriceType;
            await _priceListRepository.UpdateAsync(existingPrice);
        }
        else
        {
            var priceList = new PriceList(data.PriceType, data.Price, dateFrom, surface.Id);
            await _priceListRepository.AddAsync(priceList);
        }

        await _surfaceRepository.SaveChangesAsync();

        return surface;
    }

    public async Task<Surface> UpdateStatusAsync(int id, UpdateSurfaceStatusData data)
    {
        var surface = await _surfaceRepository.GetByIdAsync(id);
        if (surface == null)
            throw new KeyNotFoundException($"Surface with id {id} not found");

        var surfaceStatus = new SurfaceStatus(data.Status, data.DateFrom, surface.Id);
        await _surfaceStatusRepository.AddAsync(surfaceStatus);

        await _surfaceRepository.SaveChangesAsync();

        return surface;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var surface = await _surfaceRepository.GetByIdAsync(id);
        if (surface == null)
            return false;

        await _surfaceRepository.DeleteAsync(surface);
        return true;
    }

    private bool IsSurfaceAvailableForPerShow(Surface surface, FilterSurfacesData filter)
    {
        if (!filter.AvailableDaysFrom.HasValue || !filter.AvailableDaysTo.HasValue)
            return true;

        var daysFrom = filter.AvailableDaysFrom.Value.Date;
        var daysTo = filter.AvailableDaysTo.Value.Date;
        var hours = filter.AvailableHours;
        var requiredHours = hours ?? Enumerable.Range(0, 24).Select(_ => true).ToArray();

        for (var date = daysFrom; date <= daysTo; date = date.AddDays(1))
        for (var hour = 0; hour < 24; hour++)
        {
            if (!requiredHours[hour])
                continue;

            var booking = surface.Bookings?.FirstOrDefault(b => b.Date.Date == date.Date && b.Hour == hour);
            if (booking != null && booking.SlotsOccupied >= surface.MaxSlots) return false;
        }

        return true;
    }

    private bool IsSurfaceAvailableForPerMonth(Surface surface, FilterSurfacesData filter)
    {
        if (filter.AvailableMonths == null || !filter.AvailableMonths.Any())
            return true;

        var currentYear = DateTime.UtcNow.Year;

        foreach (var month in filter.AvailableMonths)
        {
            if (month < 1 || month > 12)
                continue;

            var firstDayOfMonth = new DateTime(currentYear, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                var booking = surface.Bookings?.FirstOrDefault(b => b.Date.Date == date.Date && b.Hour == 0);
                if (booking != null && booking.SlotsOccupied >= surface.MaxSlots) return false;
            }
        }

        return true;
    }
}