using AdvertisingApp.Core.Contracts;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Contracts;
using AdvertisingApp.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AdvertisingApp.Tests.Integration;

public class ClientsControllerIntegrationTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ClientsController _controller;
    private readonly IClientService _service;

    public ClientsControllerIntegrationTests()
    {
        _context = TestDbContextHelper.CreateDbContext();
        var (service, _) = ClientServiceFactory.Create(_context);
        _service = service;
        _controller = new ClientsController(_service);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllClients()
    {
        // Arrange
        await ClientFactory.CreateAndSaveAsync(_context, "Client 1");
        await ClientFactory.CreateAndSaveAsync(_context, "Client 2");

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var clients = okResult.Value.Should().BeAssignableTo<IEnumerable<ClientResponseModel>>().Subject;
        clients.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ShouldReturnClient_WhenExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);

        // Act
        var result = await _controller.GetById(client.Id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var clientResponse = okResult.Value.Should().BeOfType<ClientResponseModel>().Subject;
        clientResponse.Id.Should().Be(client.Id);
        clientResponse.Name.Should().Be("Test Client");
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
    public async Task Create_ShouldCreateClientAndReturnCreated()
    {
        // Arrange
        var request = new CreateClientData { Name = "New Client", Phone = "1234567890" };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(ClientsController.GetById));

        var client = createdResult.Value.Should().BeOfType<ClientResponseModel>().Subject;
        client.Name.Should().Be("New Client");

        // Verify in database
        var dbClient = await _context.Clients.FirstOrDefaultAsync(c => c.Name == "New Client");
        dbClient.Should().NotBeNull();
        dbClient!.Phone.Should().Be("1234567890");
    }

    [Fact]
    public async Task Update_ShouldUpdateClient_WhenExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context, "Old Name");

        var request = new UpdateClientData { Name = "Updated Name", Phone = "1111111111" };

        // Act
        var result = await _controller.Update(client.Id, request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var clientResponse = okResult.Value.Should().BeOfType<ClientResponseModel>().Subject;
        clientResponse.Name.Should().Be("Updated Name");

        // Verify in database
        var dbClient = await _context.Clients.FindAsync(client.Id);
        dbClient!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var request = new UpdateClientData { Name = "Updated Name", Phone = "1111111111" };

        // Act
        var result = await _controller.Update(999, request);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_ShouldDeleteClient_WhenExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context, "To Delete");

        // Act
        var result = await _controller.Delete(client.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        // Verify in database
        var dbClient = await _context.Clients.FindAsync(client.Id);
        dbClient.Should().BeNull();
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