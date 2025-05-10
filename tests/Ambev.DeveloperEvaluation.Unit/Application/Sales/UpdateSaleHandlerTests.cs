using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the UpdateSaleHandler class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateSaleHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();

        _handler = new UpdateSaleHandler(
            _saleRepository,
            _customerRepository,
            _branchRepository,
            _productRepository,
            _mapper,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid sale update command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should update sale successfully")]
    public async Task Handle_ValidCommand_UpdatesSaleSuccessfully()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        // Create existing sale
        var existingSale = new Sale(
            "Original-123456",
            DateTime.UtcNow.AddDays(-1),
            "OriginalCustomerId",
            "Original Customer",
            Guid.NewGuid(),
            "Original Branch"
        );
        SetPrivateProperty(existingSale, "Id", command.Id);
        SetPrivateProperty(existingSale, "Status", SaleStatus.Active);

        // Add an item to the existing sale
        existingSale.AddItem(
            Guid.NewGuid(),
            "Original Item",
            2,
            10.99m
        );

        // Setup customer and branch for update
        var customer = new Customer(
            name: command.CustomerName,
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        SetPrivateProperty(customer, "Id", command.CustomerId);

        var branch = new Branch(
            name: command.BranchName,
            address: "Branch Address",
            city: "Test City",
            state: "Test State",
            zipCode: "12345",
            phone: "+0987654321",
            email: "branch@example.com"
        );
        SetPrivateProperty(branch, "Id", command.BranchId);

        // Setup products for the items in the update command
        foreach (var item in command.Items)
        {
            var product = new Product(
                title: item.ProductName,
                price: item.UnitPrice,
                description: "Test Description",
                category: "Test Category",
                image: "test-image.jpg",
                stockQuantity: 100
            );
            SetPrivateProperty(product, "Id", item.ProductId);

            _productRepository.GetByIdAsync(item.ProductId).Returns(product);
        }

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).Returns(branch);

        var updatedSale = new Sale(
            "Updated-123456",
            command.Date,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );
        SetPrivateProperty(updatedSale, "Id", command.Id);

        foreach (var item in command.Items)
        {
            updatedSale.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice
            );
        }

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(updatedSale);

        var expectedResult = new UpdateSaleResult
        {
            Id = command.Id,
            Success = true,
            Message = "Sale updated successfully"
        };

        _mapper.Map<UpdateSaleResult>(updatedSale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Id.Should().Be(command.Id);

        // Verify repository calls
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<SaleModifiedNotification>(n => n.SaleId == command.Id),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns an error when the sale is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent sale should return error result")]
    public async Task Handle_NonExistentSale_ReturnsErrorResult()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }

    /// <summary>
    /// Tests that the handler returns an error when trying to update a cancelled sale.
    /// </summary>
    [Fact(DisplayName = "Cancelled sale should return error result")]
    public async Task Handle_CancelledSale_ReturnsErrorResult()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        // Create existing cancelled sale
        var existingSale = new Sale(
            "Original-123456",
            DateTime.UtcNow.AddDays(-1),
            "OriginalCustomerId",
            "Original Customer",
            Guid.NewGuid(),
            "Original Branch"
        );
        SetPrivateProperty(existingSale, "Id", command.Id);
        SetPrivateProperty(existingSale, "Status", SaleStatus.Cancelled);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("cancelled sale");
    }

    /// <summary>
    /// Helper method to set private property values for testing.
    /// </summary>
    private void SetPrivateProperty<T>(T instance, string propertyName, object value)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (propertyInfo != null)
        {
            propertyInfo.SetValue(instance, value);
        }
    }
}