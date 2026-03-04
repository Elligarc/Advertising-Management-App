using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AdvertisingApp.Tests.Integration;

public class ConstructionsControllerIntegrationTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ConstructionsController _controller;
    private readonly IConstructionService _service;

    public ConstructionsControllerIntegrationTests()
    {
        _context = TestDbContextHelper.CreateDbContext();
        var (service, _) = ConstructionServiceFactory.Create(_context);
        _service = service;
        _controller = new ConstructionsController(_service);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllConstructions()
    {
        // Arrange
        var (_, _, _, construction1) = await ConstructionFactory.CreateFullChainAsync(_context, "Address 1");
        var (_, _, _, construction2) = await ConstructionFactory.CreateFullChainAsync(_context, "Address 2", "City 2");

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var constructions = okResult.Value.Should().BeAssignableTo<IEnumerable<ConstructionResponseModel>>().Subject;
        constructions.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ShouldReturnConstruction_WhenExists()
    {
        // Arrange
        var (_, _, _, construction) = await ConstructionFactory.CreateFullChainAsync(_context);

        // Act
        var result = await _controller.GetById(construction.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var constructionResponse = okResult.Value.Should().BeOfType<ConstructionResponseModel>().Subject;
        constructionResponse.Id.Should().Be(construction.Id);
        constructionResponse.Address.Should().Be("Test Address");
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotExists()
    {
        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldCreateConstructionAndReturnCreated()
    {
        // Arrange
        var (_, district, format, _) = await ConstructionFactory.CreateFullChainAsync(_context, "Existing Address");

        var request = new CreateConstructionData
        {
            Address = "New Address",
            DistrictId = district.Id,
            FormatId = format.Id
        };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(ConstructionsController.GetById));

        var construction = createdResult.Value.Should().BeOfType<ConstructionResponseModel>().Subject;
        construction.Address.Should().Be("New Address");

        // Verify in database
        var dbConstruction = await _context.Constructions.FirstOrDefaultAsync(c => c.Address == "New Address");
        dbConstruction.Should().NotBeNull();
        dbConstruction!.DistrictId.Should().Be(district.Id);
        dbConstruction.FormatId.Should().Be(format.Id);
    }

    [Fact]
    public async Task Update_ShouldUpdateConstruction_WhenExists()
    {
        // Arrange
        var (_, district1, format1, construction) =
            await ConstructionFactory.CreateFullChainAsync(_context, "Old Address");
        var (_, district2, format2, _) =
            await ConstructionFactory.CreateFullChainAsync(_context, "Another Address", "City 2");

        var request = new UpdateConstructionData
        {
            Address = "Updated Address",
            DistrictId = district2.Id,
            FormatId = format2.Id
        };

        // Act
        var result = await _controller.Update(construction.Id, request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var constructionResponse = okResult.Value.Should().BeOfType<ConstructionResponseModel>().Subject;
        constructionResponse.Address.Should().Be("Updated Address");

        // Verify in database
        var dbConstruction = await _context.Constructions.FindAsync(construction.Id);
        dbConstruction!.Address.Should().Be("Updated Address");
        dbConstruction.DistrictId.Should().Be(district2.Id);
        dbConstruction.FormatId.Should().Be(format2.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var request = new UpdateConstructionData
        {
            Address = "Updated Address",
            DistrictId = 1,
            FormatId = 1
        };

        // Act
        var result = await _controller.Update(999, request);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_ShouldDeleteConstruction_WhenExists()
    {
        // Arrange
        var (_, _, _, construction) = await ConstructionFactory.CreateFullChainAsync(_context, "To Delete");

        // Act
        var result = await _controller.Delete(construction.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        // Verify in database
        var dbConstruction = await _context.Constructions.FindAsync(construction.Id);
        dbConstruction.Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenNotExists()
    {
        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}