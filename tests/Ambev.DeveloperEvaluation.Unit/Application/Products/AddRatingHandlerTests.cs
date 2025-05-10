using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Products.AddRating;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the AddRatingHandler class.
/// </summary>
public class AddRatingHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<AddRatingHandler> _logger;
    private readonly AddRatingHandler _handler;

    /// <summary>
    /// Initializes a new instance of the AddRatingHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public AddRatingHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<AddRatingHandler>>();

        _handler = new AddRatingHandler(
            _productRepository,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid rating addition command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should add rating successfully")]
    public async Task Handle_ValidCommand_AddsRatingSuccessfully()
    {
        // Arrange
        var command = AddRatingHandlerTestData.GenerateValidCommand();

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
        _productRepository.AddRatingAsync(command.Id, command.Rating).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.Received(1).AddRatingAsync(command.Id, command.Rating);
    }

    /// <summary>
    /// Tests that the handler throws an exception when rating is invalid.
    /// </summary>
    [Fact(DisplayName = "Invalid rating should throw ValidationException")]
    public async Task Handle_InvalidRating_ThrowsValidationException()
    {
        // Arrange
        var command = AddRatingHandlerTestData.GenerateCommandWithInvalidRating();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Rating must be between 0 and 5.");

        // Verify repository not called
        await _productRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
        await _productRepository.DidNotReceive().AddRatingAsync(Arg.Any<Guid>(), Arg.Any<decimal>());
    }

    /// <summary>
    /// Tests that the handler throws an exception when product is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent product should throw NotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = AddRatingHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found.");

        // Verify repository not called for add rating
        await _productRepository.Received(1).GetByIdAsync(command.Id);
        await _productRepository.DidNotReceive().AddRatingAsync(Arg.Any<Guid>(), Arg.Any<decimal>());
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