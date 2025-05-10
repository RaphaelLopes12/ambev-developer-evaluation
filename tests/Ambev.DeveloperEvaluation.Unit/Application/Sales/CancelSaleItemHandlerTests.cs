using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the CancelSaleItemHandler class.
/// </summary>
public class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleItemHandler> _logger;
    private readonly CancelSaleItemHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CancelSaleItemHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleItemHandler>>();

        _handler = new CancelSaleItemHandler(
            _saleRepository,
            _productRepository,
            _mediator,
            _mapper,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid sale item cancellation command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should cancel sale item successfully")]
    public async Task Handle_ValidCommand_CancelsSaleItemSuccessfully()
    {
        // Arrange
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        // Create a sale with the item to be cancelled
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.SaleId);

        // Add the item to be cancelled
        sale.AddItem(
            command.ProductId,
            "Product to Cancel",
            2,
            10.99m
        );

        // Add another item that won't be cancelled
        var otherProductId = Guid.NewGuid();
        sale.AddItem(
            otherProductId,
            "Other Product",
            3,
            15.99m
        );

        // Create a product for the product ID
        var product = new Product(
            "Product to Cancel",
            10.99m,
            "Description",
            "Category",
            "image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.ProductId);

        // Setup repository mocks
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId).Returns(product);

        // Calculate the new total after cancellation
        decimal newTotal = (15.99m * 3); // Only the other item's total

        // When updating sale, return a sale with cancelled item
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var updatedSale = (Sale)callInfo.Arg<Sale>();
                // Check that the item is cancelled
                var cancelledItem = updatedSale.Items.First(i => i.ProductId == command.ProductId);
                cancelledItem.IsCancelled.Should().BeTrue();
                return updatedSale;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.SaleId.Should().Be(command.SaleId);
        result.ProductId.Should().Be(command.ProductId);
        result.Message.Should().Contain("cancelled successfully");

        // Verify repository calls
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<ItemCancelledNotification>(n =>
                n.SaleId == command.SaleId &&
                n.ProductId == command.ProductId),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns an error result when the sale is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent sale should return error result")]
    public async Task Handle_NonExistentSale_ReturnsErrorResult()
    {
        // Arrange
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");

        // Verify repository not called for update
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ItemCancelledNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns an error result when the product is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent product should return error result")]
    public async Task Handle_NonExistentProduct_ReturnsErrorResult()
    {
        // Arrange
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        // Create sale
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.SaleId);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");

        // Verify repository not called for update
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ItemCancelledNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns an error result when the sale doesn't have the item.
    /// </summary>
    [Fact(DisplayName = "Item not in sale should return error result")]
    public async Task Handle_ItemNotInSale_ReturnsErrorResult()
    {
        // Arrange
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        // Create sale without the item to be cancelled
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.SaleId);

        // Add a different item
        var otherProductId = Guid.NewGuid();
        sale.AddItem(
            otherProductId,
            "Other Product",
            3,
            15.99m
        );

        // Create product
        var product = new Product(
            "Product to Cancel",
            10.99m,
            "Description",
            "Category",
            "image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.ProductId);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found in sale");

        // Verify repository not called for update
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ItemCancelledNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns an error result when the item is already cancelled.
    /// </summary>
    [Fact(DisplayName = "Already cancelled item should return error result")]
    public async Task Handle_AlreadyCancelledItem_ReturnsErrorResult()
    {
        // Arrange
        var command = CancelSaleItemHandlerTestData.GenerateValidCommand();

        // Create sale with the item to be cancelled
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.SaleId);

        // Add the item
        sale.AddItem(
            command.ProductId,
            "Product to Cancel",
            2,
            10.99m
        );

        // Cancel the item manually
        sale.CancelItem(command.ProductId);

        // Create product
        var product = new Product(
            "Product to Cancel",
            10.99m,
            "Description",
            "Category",
            "image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.ProductId);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(command.ProductId).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("already cancelled");

        // Verify repository not called for update
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ItemCancelledNotification>(),
            Arg.Any<CancellationToken>()
        );
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