using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using CoreContracts = AdvertisingApp.Core.Contracts;

namespace AdvertisingApp.Tests.Integration.Surfaces;

public class SurfacesControllerFilterTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SurfacesController _controller;
    private readonly ISurfaceService _service;

    public SurfacesControllerFilterTests()
    {
        _context = TestDbContextHelper.CreateDbContext();
        var (service, _) = SurfaceServiceFactory.Create(_context);
        _service = service;
        _controller = new SurfacesController(_service);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task Filter_ShouldReturnSurfacesByCity()
    {
        // Arrange
        var surfaces = await SurfaceFactory.CreateMultipleWithDifferentCitiesAsync(
            _context, "Moscow", "Tomsk");

        // Act
        var filter = new CoreContracts.FilterSurfacesData { CityId = surfaces[0].Construction!.District!.CityId };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var filteredSurfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        filteredSurfaces.Should().HaveCount(1);
    }

    [Fact]
    public async Task Filter_ShouldReturnSurfacesByPriceType()
    {
        // Arrange
        var surfaces = await SurfaceFactory.CreateMultipleWithDifferentPriceTypesAsync(
            _context, PriceType.PerShow, PriceType.PerMonth);

        // Act
        var filter = new CoreContracts.FilterSurfacesData { PriceType = PriceType.PerShow };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var filteredSurfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        filteredSurfaces.Should().HaveCount(1);
        filteredSurfaces.First().CurrentPriceType.Should().Be(PriceType.PerShow);
    }

    [Fact]
    public async Task Filter_ShouldReturnSurfacesByConstructionType()
    {
        // Arrange
        var surfaces = await SurfaceFactory.CreateMultipleWithDifferentConstructionTypesAsync(
            _context, ConstructionType.Billboard, ConstructionType.Videoboard);

        // Act
        var filter = new CoreContracts.FilterSurfacesData { ConstructionType = ConstructionType.Billboard };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var filteredSurfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        filteredSurfaces.Should().HaveCount(1);
    }

    [Fact]
    public async Task Filter_ShouldReturnAvailableSurfacesForPerShow()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            price: 100m,
            priceType: PriceType.PerShow,
            priceDate: DateTime.UtcNow.AddDays(-1),
            statusDate: DateTime.UtcNow.AddDays(-1));

        var availableHours = new bool[24];
        for (var i = 0; i < 24; i++)
            availableHours[i] = true;

        // Act
        var filter = new CoreContracts.FilterSurfacesData
        {
            PriceType = PriceType.PerShow,
            AvailableDaysFrom = DateTime.UtcNow.AddDays(1),
            AvailableDaysTo = DateTime.UtcNow.AddDays(2),
            AvailableHours = availableHours
        };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        surfaces.Should().HaveCount(1);
    }

    [Fact]
    public async Task Filter_ShouldNotReturnOccupiedSurfacesForPerShow()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateWithBookingAsync(
            _context,
            price: 100m,
            priceType: PriceType.PerShow,
            slotsOccupied: 6);

        var availableHours = new bool[24];
        for (var i = 0; i < 24; i++)
            availableHours[i] = true;

        // Act
        var filter = new CoreContracts.FilterSurfacesData
        {
            PriceType = PriceType.PerShow,
            AvailableDaysFrom = DateTime.UtcNow.AddDays(1),
            AvailableDaysTo = DateTime.UtcNow.AddDays(2),
            AvailableHours = availableHours
        };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        surfaces.Should().BeEmpty();
    }

    [Fact]
    public async Task Filter_ShouldReturnAvailableSurfacesForPerMonth()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            price: 5000m,
            priceType: PriceType.PerMonth,
            priceDate: DateTime.UtcNow.AddDays(-1),
            statusDate: DateTime.UtcNow.AddDays(-1));

        // Act
        var filter = new CoreContracts.FilterSurfacesData
        {
            PriceType = PriceType.PerMonth,
            AvailableMonths = new List<int> { 1, 2, 3 }
        };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        surfaces.Should().HaveCount(1);
    }

    [Fact]
    public async Task Filter_ShouldNotReturnOccupiedSurfacesForPerMonth()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateWithBookingAsync(
            _context,
            price: 5000m,
            priceType: PriceType.PerMonth,
            bookingDate: new DateTime(DateTime.UtcNow.Year, 1, 15),
            slotsOccupied: 10);

        // Act
        var filter = new CoreContracts.FilterSurfacesData
        {
            PriceType = PriceType.PerMonth,
            AvailableMonths = new List<int> { 1, 2, 3 }
        };
        var result = await _controller.Filter(filter);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        surfaces.Should().BeEmpty();
    }
}