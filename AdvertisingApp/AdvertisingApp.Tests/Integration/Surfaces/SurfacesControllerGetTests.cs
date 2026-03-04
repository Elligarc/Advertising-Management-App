using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AdvertisingApp.Tests.Integration.Surfaces;

public class SurfacesControllerGetTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SurfacesController _controller;
    private readonly ISurfaceService _service;

    public SurfacesControllerGetTests()
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
    public async Task GetByConstruction_ShouldReturnSurfaces_WhenExists()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            "Test Address");

        // Verify surface was created
        surface.Id.Should().BeGreaterThan(0);
        surface.ConstructionId.Should().BeGreaterThan(0);

        // Act - call controller with named parameter to ensure binding
        var result = await _controller.GetByConstruction(surface.ConstructionId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaces = okResult.Value.Should().BeAssignableTo<IEnumerable<SurfaceResponseModel>>().Subject;
        surfaces.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetById_ShouldReturnSurface_WhenExists()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital);

        // Act
        var result = await _controller.GetById(surface.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var surfaceResponse = okResult.Value.Should().BeOfType<SurfaceResponseModel>().Subject;
        surfaceResponse.Id.Should().Be(surface.Id);
        surfaceResponse.Side.Should().Be(Side.A);
    }
}