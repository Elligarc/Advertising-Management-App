using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CoreContracts = AdvertisingApp.Core.Contracts;

namespace AdvertisingApp.Tests.Integration.Surfaces;

public class SurfacesControllerUpdateTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SurfacesController _controller;
    private readonly ISurfaceService _service;

    public SurfacesControllerUpdateTests()
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
    public async Task UpdatePrice_ShouldCreateNewPriceListEntry()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            price: 100m,
            priceType: PriceType.PerShow);

        var request = new CoreContracts.UpdateSurfacePriceData
        {
            Price = 200m,
            PriceType = PriceType.PerMonth,
            DateFrom = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _controller.UpdatePrice(surface.Id, request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

        // Verify new price list entry
        var priceLists = await _context.PriceLists.Where(p => p.SurfaceId == surface.Id).ToListAsync();
        priceLists.Should().HaveCount(2);
        priceLists.Last().Price.Should().Be(200m);
    }

    [Fact]
    public async Task UpdateStatus_ShouldCreateNewStatusEntry()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            status: Status.Created);

        var request = new CoreContracts.UpdateSurfaceStatusData
        {
            Status = Status.UnderRepair,
            DateFrom = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _controller.UpdateStatus(surface.Id, request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

        // Verify new status entry
        var statuses = await _context.SurfaceStatuses.Where(s => s.SurfaceId == surface.Id).ToListAsync();
        statuses.Should().HaveCount(2);
        statuses.Last().Status.Should().Be(Status.UnderRepair);
    }
}