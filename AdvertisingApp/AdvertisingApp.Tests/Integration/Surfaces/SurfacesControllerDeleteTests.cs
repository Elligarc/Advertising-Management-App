using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AdvertisingApp.Tests.Integration.Surfaces;

public class SurfacesControllerDeleteTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SurfacesController _controller;
    private readonly ISurfaceService _service;

    public SurfacesControllerDeleteTests()
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
    public async Task Delete_ShouldDeleteSurface_WhenExists()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital);

        // Act
        var result = await _controller.Delete(surface.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        // Verify in database
        var dbSurface = await _context.Surfaces.FindAsync(surface.Id);
        dbSurface.Should().BeNull();
    }
}