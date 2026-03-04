using AdvertisingApp.Core.Entities;
using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CoreContracts = AdvertisingApp.Core.Contracts;

namespace AdvertisingApp.Tests.Integration.Surfaces;

public class SurfacesControllerCreateTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SurfacesController _controller;
    private readonly ISurfaceService _service;

    public SurfacesControllerCreateTests()
    {
        _context = TestDbContextHelper.CreateDbContext();
        var (service, _) = CreateSurfaceService(_context);
        _service = service;
        _controller = new SurfacesController(_service);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private static (ISurfaceService Service, AppDbContext Context) CreateSurfaceService(AppDbContext context)
    {
        var constructionRepo = new EfRepository<Construction>(context);
        var surfaceRepo = new EfRepository<Surface>(context);
        var priceListRepo = new EfRepository<PriceList>(context);
        var surfaceStatusRepo = new EfRepository<SurfaceStatus>(context);
        var surfaceBookingRepo = new EfRepository<SurfaceBooking>(context);

        var service = new SurfaceService(surfaceRepo, constructionRepo, priceListRepo, surfaceStatusRepo,
            surfaceBookingRepo);
        return (service, context);
    }

    [Fact]
    public async Task Create_ShouldCreateSurfaceWithPriceListAndStatus()
    {
        // Arrange
        var (_, _, _, construction) = await ConstructionFactory.CreateFullChainAsync(
            _context);

        var request = new CoreContracts.CreateSurfaceData
        {
            ConstructionId = construction.Id,
            Side = Side.A,
            SurfaceType = SurfaceType.Digital,
            LoopDuration = 60,
            SlotDuration = 10,
            InitialPrice = 1000m,
            PriceType = PriceType.PerShow
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var surfaceResponse = createdResult.Value.Should().BeOfType<SurfaceResponseModel>().Subject;
        surfaceResponse.Side.Should().Be(Side.A);
        surfaceResponse.SurfaceType.Should().Be(SurfaceType.Digital);

        // Verify price list created
        var dbPriceList = await _context.PriceLists.FirstOrDefaultAsync(p => p.SurfaceId == surfaceResponse.Id);
        dbPriceList.Should().NotBeNull();
        dbPriceList!.Price.Should().Be(1000m);

        // Verify status created
        var dbStatus = await _context.SurfaceStatuses.FirstOrDefaultAsync(s => s.SurfaceId == surfaceResponse.Id);
        dbStatus.Should().NotBeNull();
        dbStatus!.Status.Should().Be(Status.Created);
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenSameSideExists()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context);

        var request = new CoreContracts.CreateSurfaceData
        {
            ConstructionId = surface.ConstructionId,
            Side = Side.A, // Same side
            SurfaceType = SurfaceType.Digital,
            LoopDuration = 60,
            SlotDuration = 10,
            InitialPrice = 1000m,
            PriceType = PriceType.PerShow
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}