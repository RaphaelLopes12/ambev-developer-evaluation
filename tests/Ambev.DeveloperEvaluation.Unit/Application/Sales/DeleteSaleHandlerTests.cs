using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the DeleteSaleHandler class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteSaleHandler> _logger;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the DeleteSaleHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<DeleteSaleHandler>>();

        _handler = new DeleteSaleHandler(
            _saleRepository,
            _productRepository,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid sale deletion command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should delete sale successfully")]
    public async Task Handle_ValidCommand_DeletesSaleSuccessfully()
    {
        // Arrange
        var command = DeleteSaleHandlerTestData.GenerateValidCommand();

        // Create a sale with a few items
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.Id);

        // Add some items to the sale
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        sale.AddItem(
            productId1,
            "Product 1",
            2,
            10.99m
        );

        sale.AddItem(
            productId2,
            "Product 2",
            3,
            15.99m
        );

        // Setup product repository mocks
        var product1 = new Product(
            "Product 1",
            10.99m,
            "Description 1",
            "Category 1",
            "image1.jpg",
            50
        );
        SetPrivateProperty(product1, "Id", productId1);

        var product2 = new Product(
            "Product 2",
            15.99m,
            "Description 2",
            "Category 2",
            "image2.jpg",
            30
        );
        SetPrivateProperty(product2, "Id", productId2);

        _productRepository.GetByIdAsync(productId1).Returns(product1);
        _productRepository.GetByIdAsync(productId2).Returns(product2);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<SaleDeletedNotification>(n => n.SaleId == command.Id),
            Arg.Any<CancellationToken>()
        );

        // Verify stock was restored for each item
        await _productRepository.Received(1).UpdateStockAsync(
            productId1,
            Arg.Any<int>()
        );

        await _productRepository.Received(1).UpdateStockAsync(
            productId2,
            Arg.Any<int>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when the sale is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent sale should return false")]
    public async Task Handle_NonExistentSale_ReturnsFalse()
    {
        // Arrange
        var command = DeleteSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository not called for deletion
        await _saleRepository.DidNotReceive().DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<SaleDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when the repository fails to delete the sale.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = DeleteSaleHandlerTestData.GenerateValidCommand();

        // Create sale
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository called
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<SaleDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler handles exceptions gracefully.
    /// </summary>
    [Fact(DisplayName = "Repository exception should be handled")]
    public async Task Handle_RepositoryException_ReturnsFalse()
    {
        // Arrange
        var command = DeleteSaleHandlerTestData.GenerateValidCommand();

        // Create sale
        var sale = new Sale(
            "Sale-123456",
            DateTime.UtcNow.AddDays(-1),
            "CustomerId123",
            "Test Customer",
            Guid.NewGuid(),
            "Test Branch"
        );
        SetPrivateProperty(sale, "Id", command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
            .Throws(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository called
        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());

        // Verify notification not published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<SaleDeletedNotification>(),
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