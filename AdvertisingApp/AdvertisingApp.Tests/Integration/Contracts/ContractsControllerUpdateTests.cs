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

namespace AdvertisingApp.Tests.Integration.Contracts;

public class ContractsControllerUpdateTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ContractsController _controller;
    private readonly IContractService _service;

    public ContractsControllerUpdateTests()
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
    public async Task Update_ShouldReturnOk_WhenActivateContract()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id, status: ContractStatus.Created);

        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Active };

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Status.Should().Be(ContractStatus.Active);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenCancelContract()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id, status: ContractStatus.Created);

        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Cancelled };

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Status.Should().Be(ContractStatus.Cancelled);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenContractNotExists()
    {
        // Arrange
        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Active };

        // Act
        var result = await _controller.Update(999, data);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenContractIsActive()
    {
        // Arrange
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            status: ContractStatus.Active);

        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Cancelled };

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenContractIsCancelled()
    {
        // Arrange
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            status: ContractStatus.Cancelled);

        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Active };

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldSucceed_WithActiveToCancelled_IfNotStarted()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(
            _context,
            client.Id,
            DateTime.UtcNow.AddDays(10),
            DateTime.UtcNow.AddMonths(2),
            status: ContractStatus.Active);

        var data = new CoreContracts.UpdateContractData { Status = ContractStatus.Cancelled };

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_ShouldNotChangeStatus_WhenStatusNotProvided()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id, status: ContractStatus.Created);

        var data = new CoreContracts.UpdateContractData();

        // Act
        var result = await _controller.Update(contract.Id, data);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Status.Should().Be(ContractStatus.Created);
    }
}