using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
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
/// Contains unit tests for the UpdateProductHandler class.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly UpdateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateProductHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<UpdateProductHandler>>();

        _handler = new UpdateProductHandler(
            _productRepository,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid update product command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should update product successfully")]
    public async Task Handle_ValidCommand_UpdatesProductSuccessfully()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        // Create original product
        var originalProduct = new Product(
            "Original Title",
            9.99m,
            "Original Description",
            "Original Category",
            "original-image.jpg",
            50
        );
        SetPrivateProperty(originalProduct, "Id", command.Id);

        // Create updated product
        var updatedProduct = new Product(
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image,
            50 // Keeping the same stock quantity
        );
        SetPrivateProperty(updatedProduct, "Id", command.Id);

        // Setup mocks
        _productRepository.GetByIdAsync(command.Id).Returns(originalProduct, updatedProduct);
        _productRepository.UpdateDetailsAsync(
            command.Id,
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image
        ).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Title.Should().Be(command.Title);
        result.Price.Should().Be(command.Price);
        result.Category.Should().Be(command.Category);

        // Verify repository calls
        await _productRepository.Received(2).GetByIdAsync(command.Id);
        await _productRepository.Received(1).UpdateDetailsAsync(
            command.Id,
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image
        );

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<ProductUpdatedNotification>(n =>
                n.Id == command.Id &&
                n.Title == command.Title),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when product is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent product should throw NotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found.");

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().UpdateDetailsAsync(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            Arg.Any<decimal>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when validation fails.
    /// </summary>
    [Fact(DisplayName = "Invalid product should throw ValidationException")]
    public async Task Handle_InvalidProduct_ThrowsValidationException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateCommandWithInvalidPrice();

        // Create product
        var product = new Product(
            "Original Title",
            9.99m,
            "Original Description",
            "Original Category",
            "original-image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.Id);

        // Set up mock
        _productRepository.GetByIdAsync(command.Id).Returns(product);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().UpdateDetailsAsync(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            Arg.Any<decimal>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when repository update fails.
    /// </summary>
    [Fact(DisplayName = "Repository failure should throw ApplicationException")]
    public async Task Handle_RepositoryFailure_ThrowsApplicationException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        // Create product
        var product = new Product(
            "Original Title",
            9.99m,
            "Original Description",
            "Original Category",
            "original-image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.Id);

        // Set up mocks
        _productRepository.GetByIdAsync(command.Id).Returns(product);
        _productRepository.UpdateDetailsAsync(
            command.Id,
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image
        ).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage($"Failed to update product with ID {command.Id}.");

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).UpdateDetailsAsync(
            command.Id,
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image
        );

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductUpdatedNotification>(),
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