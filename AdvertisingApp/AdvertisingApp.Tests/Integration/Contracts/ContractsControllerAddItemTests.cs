using AdvertisingApp.Core.Enums;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Tests.Helpers;
using AdvertisingApp.Web.Controllers;
using AdvertisingApp.Web.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CoreContracts = AdvertisingApp.Core.Contracts;
using SurfaceFactory = AdvertisingApp.Tests.Helpers.SurfaceFactory;

namespace AdvertisingApp.Tests.Integration.Contracts;

public class ContractsControllerAddItemTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ContractsController _controller;
    private readonly IContractService _service;

    public ContractsControllerAddItemTests()
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
    public async Task AddItem_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var newSurface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            price: 10000m,
            priceType: PriceType.PerMonth);

        var contract = await ContractFactory.CreateAndSaveAsync(
            _context,
            clientId: (await ClientFactory.CreateAndSaveAsync(_context)).Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = newSurface.Id,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 10000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.RouteValues!["id"].Should().Be(contract.Id);
    }

    [Fact]
    public async Task AddItem_ShouldReturnNotFound_WhenContractNotExists()
    {
        // Arrange
        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = 1,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 1000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(999, data);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldReturnBadRequest_WhenContractIsActive()
    {
        // Arrange
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            status: ContractStatus.Active);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = contract.Items.First().SurfaceId,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 10000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldReturnBadRequest_WhenContractIsCancelled()
    {
        // Arrange
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            status: ContractStatus.Cancelled);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = contract.Items.First().SurfaceId,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 10000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldReturnNotFound_WhenSurfaceNotExists()
    {
        // Arrange
        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = 999,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 1000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldReturnBadRequest_WhenSurfaceUnavailable()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateWithBookingAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            slotsOccupied: 10); // Max slots

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.AddDays(1),
            Price = 100m,
            PriceType = PriceType.PerShow,
            DaysOfWeek = new[] { 0, 1, 2, 3, 4, 5, 6 },
            HoursInDay = new[] { 0 }
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldSucceed_WithDigitalSurfaceAndSchedule()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            price: 100m,
            priceType: PriceType.PerShow,
            loopDuration: 60,
            slotDuration: 10);

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 100m,
            PriceType = PriceType.PerShow,
            DaysOfWeek = new[] { 0, 1, 2, 3, 4 },
            HoursInDay = new[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractItemResponseModel>().Subject;
        response.SurfaceId.Should().Be(surface.Id);
    }

    [Fact]
    public async Task AddItem_ShouldSucceed_WithRegularSurfacePerMonth()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            price: 15000m,
            priceType: PriceType.PerMonth);

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(2),
            Price = 15000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractItemResponseModel>().Subject;
        response.PriceType.Should().Be(PriceType.PerMonth);
    }

    [Fact]
    public async Task AddItem_ShouldCreateBooking_WhenSuccessful()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            price: 10000m,
            priceType: PriceType.PerMonth);

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(3),
            Price = 1000m,
            PriceType = PriceType.PerMonth,
            DaysOfWeek = new[] { 0, 1, 2, 3, 4, 5, 6 },
            HoursInDay = new[] { 10, 11, 12 }
        };

        // Act
        await _controller.AddItem(contract.Id, data);

        // Assert
        var bookings = await _context.SurfaceBookings.Where(b => b.SurfaceId == surface.Id).ToListAsync();
        bookings.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddItem_ShouldCalculateCorrectTotalPrice_WhenPerMonth()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            price: 10000m,
            priceType: PriceType.PerMonth);

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var endDate = startDate.AddMonths(2); // ~2 months
        var pricePerMonth = 10000m;

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = startDate,
            EndDate = endDate,
            Price = pricePerMonth,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractItemResponseModel>().Subject;

        // Expected: 2 months * 10000 = 20000
        response.TotalPrice.Should().Be(20000m);

        var updatedContract = await _context.Contracts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == contract.Id);
        updatedContract!.TotalPrice.Should().Be(20000m);
    }

    [Fact]
    public async Task AddItem_ShouldCalculateCorrectTotalPrice_WhenPerShow()
    {
        // Arrange
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            price: 1m, // 1 price per show
            priceType: PriceType.PerShow,
            loopDuration: 60,
            slotDuration: 10); // 10 seconds per slot = 360 slots per hour

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var endDate = startDate.AddDays(1); // 2 days
        var pricePerShow = 1m;

        // 5 working days (Monday-Friday), 8 hours per day
        var daysOfWeek = new[] { 0, 1, 2, 3, 4 }; // Monday-Friday
        var hoursInDay = new[] { 9, 10, 11, 12, 13, 14, 15, 16 }; // 9am-4pm

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = startDate,
            EndDate = endDate,
            Price = pricePerShow,
            PriceType = PriceType.PerShow,
            DaysOfWeek = daysOfWeek,
            HoursInDay = hoursInDay
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ContractItemResponseModel>().Subject;

        // Expected: 360 cycles/hour * 8 hours * 2 days * 1 rouble = 5760
        response.TotalPrice.Should().Be(5760m);
    }

    [Fact]
    public async Task AddItem_ShouldReturnBadRequest_WhenSurfaceAlreadyInContract()
    {
        // Arrange - create contract with surface already
        var contract = await ContractFactory.CreateWithSurfaceAsync(
            _context,
            Side.A,
            SurfaceType.Regular,
            status: ContractStatus.Created,
            price: 10000m,
            priceType: PriceType.PerMonth);

        var existingSurfaceId = contract.Items.First().SurfaceId;

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = existingSurfaceId,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(1),
            Price = 10000m,
            PriceType = PriceType.PerMonth
        };

        // Act
        var result = await _controller.AddItem(contract.Id, data);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task AddItem_ShouldCreateSurfaceBooking_WithCorrectSlotsOccupied()
    {
        // Arrange - Digital surface with PerShow pricing
        var surface = await SurfaceFactory.CreateAndSaveAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            price: 100m,
            priceType: PriceType.PerShow,
            loopDuration: 60,
            slotDuration: 10);

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var endDate = startDate.AddDays(1);

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = startDate,
            EndDate = endDate,
            Price = 100m,
            PriceType = PriceType.PerShow,
            DaysOfWeek = new[] { 0, 1, 2, 3, 4, 5, 6 },
            HoursInDay = new[] { 10, 11 }
        };

        // Act
        await _controller.AddItem(contract.Id, data);

        // Assert - verify bookings were created with SlotsOccupied = 1
        var bookings = await _context.SurfaceBookings
            .Where(b => b.SurfaceId == surface.Id)
            .ToListAsync();

        bookings.Should().NotBeEmpty();
        bookings.Should().AllSatisfy(b => b.SlotsOccupied.Should().Be(1));
    }

    [Fact]
    public async Task AddItem_ShouldIncrementSlotsOccupied_WhenBookingExists()
    {
        // Arrange - create surface with existing booking
        var surface = await SurfaceFactory.CreateWithBookingAsync(
            _context,
            Side.A,
            SurfaceType.Digital,
            price: 100m,
            priceType: PriceType.PerShow,
            slotDuration: 10,
            bookingDate: DateTime.UtcNow.AddDays(1),
            slotsOccupied: 1); // Already has 1 slot occupied

        var client = await ClientFactory.CreateAndSaveAsync(_context);
        var contract = await ContractFactory.CreateAndSaveAsync(_context, client.Id);

        var startDate = DateTime.UtcNow.AddDays(1).Date;
        var endDate = startDate;

        var data = new CoreContracts.CreateContractItemData
        {
            SurfaceId = surface.Id,
            StartDate = startDate,
            EndDate = endDate,
            Price = 100m,
            PriceType = PriceType.PerShow,
            DaysOfWeek = new[] { 0, 1, 2, 3, 4, 5, 6 },
            HoursInDay = new[] { 0 } // Same hour as existing booking
        };

        // Act
        await _controller.AddItem(contract.Id, data);

        // Assert - verify SlotsOccupied increased to 2
        var booking = await _context.SurfaceBookings
            .FirstOrDefaultAsync(b => b.SurfaceId == surface.Id && b.Hour == 0);

        booking.Should().NotBeNull();
        booking!.SlotsOccupied.Should().Be(2);
    }
}
