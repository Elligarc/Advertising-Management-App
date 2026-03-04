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

public class ContractsControllerCreateTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ContractsController _controller;
    private readonly IContractService _service;

    public ContractsControllerCreateTests()
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
    public async Task Create_ShouldReturnCreated_WhenValidClientId()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var data = new CoreContracts.CreateContractData
        {
            ClientId = client.Id
        };

        // Act
        var result = await _controller.Create(data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.ClientId.Should().Be(client.Id);
        response.Status.Should().Be(ContractStatus.Created);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenClientNotExists()
    {
        // Arrange
        var data = new CoreContracts.CreateContractData
        {
            ClientId = 999
        };

        // Act
        var result = await _controller.Create(data);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_ShouldSetStatusToCreated_ByDefault()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var data = new CoreContracts.CreateContractData
        {
            ClientId = client.Id
        };

        // Act
        var result = await _controller.Create(data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractResponseModel>().Subject;
        response.Status.Should().Be(ContractStatus.Created);
    }
}