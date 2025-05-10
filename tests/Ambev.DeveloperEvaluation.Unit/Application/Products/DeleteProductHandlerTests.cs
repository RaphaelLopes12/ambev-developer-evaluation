using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Reflection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the DeleteProductHandler class.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteProductHandler> _logger;
    private readonly DeleteProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the DeleteProductHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<DeleteProductHandler>>();

        _handler = new DeleteProductHandler(
            _productRepository,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid delete product command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should delete product successfully")]
    public async Task Handle_ValidCommand_DeletesProductSuccessfully()
    {
        // Arrange
        var command = DeleteProductHandlerTestData.GenerateValidCommand();

        // Create product
        var product = new Product(
            "Test Product",
            19.99m,
            "Test Description",
            "Test Category",
            "test-image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.Id);

        _productRepository.GetByIdAsync(command.Id).Returns(product);
        _productRepository.RemoveAsync(command.Id).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).RemoveAsync(command.Id);

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<ProductDeletedNotification>(n =>
                n.Id == command.Id &&
                n.Title == product.Title),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when the product does not exist.
    /// </summary>
    [Fact(DisplayName = "Non-existent product should return false")]
    public async Task Handle_NonExistentProduct_ReturnsFalse()
    {
        // Arrange
        var command = DeleteProductHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().RemoveAsync(Arg.Any<Guid>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when the repository fails to delete the product.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = DeleteProductHandlerTestData.GenerateValidCommand();

        // Create product
        var product = new Product(
            "Test Product",
            19.99m,
            "Test Description",
            "Test Category",
            "test-image.jpg",
            50
        );
        SetPrivateProperty(product, "Id", command.Id);

        _productRepository.GetByIdAsync(command.Id).Returns(product);
        _productRepository.RemoveAsync(command.Id).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).RemoveAsync(command.Id);

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductDeletedNotification>(),
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