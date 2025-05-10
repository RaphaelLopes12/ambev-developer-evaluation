using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the UpdateStockHandler class.
/// </summary>
public class UpdateStockHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateStockHandler> _logger;
    private readonly UpdateStockHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateStockHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public UpdateStockHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<UpdateStockHandler>>();

        _handler = new UpdateStockHandler(
            _productRepository,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid stock update command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should update stock successfully")]
    public async Task Handle_ValidCommand_UpdatesStockSuccessfully()
    {
        // Arrange
        var command = UpdateStockHandlerTestData.GenerateValidCommand();
        var initialStockQuantity = 20;

        // Create product
        var product = new Product(
            "Test Product",
            19.99m,
            "Test Description",
            "Test Category",
            "test-image.jpg",
            initialStockQuantity
        );
        SetPrivateProperty(product, "Id", command.Id);

        _productRepository.GetByIdAsync(command.Id).Returns(product);
        _productRepository.UpdateStockAsync(command.Id, command.Quantity).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).UpdateStockAsync(command.Id, command.Quantity);

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<ProductStockUpdatedNotification>(n =>
                n.Id == command.Id &&
                n.Title == product.Title &&
                n.PreviousQuantity == initialStockQuantity &&
                n.NewQuantity == command.Quantity),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when the quantity is negative.
    /// </summary>
    [Fact(DisplayName = "Negative quantity should throw ValidationException")]
    public async Task Handle_NegativeQuantity_ThrowsValidationException()
    {
        // Arrange
        var command = UpdateStockHandlerTestData.GenerateCommandWithNegativeQuantity();

        var product = new Product(
            "Test Product",
            19.99m,
            "Test Description",
            "Test Category",
            "test-image.jpg",
            20
        );
        SetPrivateProperty(product, "Id", command.Id);

        _productRepository.GetByIdAsync(command.Id).Returns(product);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Stock quantity cannot be negative.");

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().UpdateStockAsync(Arg.Any<Guid>(), Arg.Any<int>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductStockUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when the product is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent product should throw NotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = UpdateStockHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found.");

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().UpdateStockAsync(Arg.Any<Guid>(), Arg.Any<int>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductStockUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when repository update fails.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = UpdateStockHandlerTestData.GenerateValidCommand();
        var initialStockQuantity = 20;

        // Create product
        var product = new Product(
            "Test Product",
            19.99m,
            "Test Description",
            "Test Category",
            "test-image.jpg",
            initialStockQuantity
        );
        SetPrivateProperty(product, "Id", command.Id);

        _productRepository.GetByIdAsync(command.Id).Returns(product);
        _productRepository.UpdateStockAsync(command.Id, command.Quantity).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).UpdateStockAsync(command.Id, command.Quantity);

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductStockUpdatedNotification>(),
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