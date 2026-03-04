using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AdvertisingApp.Tests.Integration.Contracts;

public class ContractsControllerGetTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ContractsController _controller;
    private readonly IContractService _service;

    public ContractsControllerGetTests()
    {
        _context = TestDbContextHelper.CreateDbContext();
        var (service, _) = ContractServiceFactory.Create(_context);
        _service = service;
        _controller = new ContractsController(_service);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_ShouldReturnContracts_WhenExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        await ContractFactory.CreateAndSaveAsync(_context, client.Id, status: ContractStatus.Active);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var contracts = okResult.Value.Should().BeAssignableTo<IEnumerable<ContractResponseModel>>().Subject;
        contracts.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoContracts()
    {
        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var contracts = okResult.Value.Should().BeAssignableTo<IEnumerable<ContractResponseModel>>().Subject;
        contracts.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ShouldReturnContract_WhenExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id, status: ContractStatus.Active);

        // Act
        var result = await _controller.GetById(contract.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Id.Should().Be(contract.Id);
        response.ClientId.Should().Be(client.Id);
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
    public async Task GetById_ShouldIncludeItems_WhenContractHasItems()
    {
        // Arrange
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            status: ContractStatus.Active);

        // Act
        var result = await _controller.GetById(contract.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Items.Should().NotBeEmpty();
    }
}