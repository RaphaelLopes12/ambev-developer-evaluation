using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the CreateProductHandler class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<CreateProductHandler>>();

        _handler = new CreateProductHandler(
            _productRepository,
            _mediator,
            _logger
        );
    }

    /// <summary>
    /// Tests that a valid create product command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should create product successfully")]
    public async Task Handle_ValidCommand_CreatesProductSuccessfully()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        _productRepository.AddAsync(Arg.Any<Product>()).Returns(callInfo =>
        {
            var product = callInfo.Arg<Product>();
            return product;
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Category.Should().Be(command.Category);

        // Verify repository was called
        await _productRepository.Received(1).AddAsync(Arg.Any<Product>());

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<ProductCreatedNotification>(n =>
                n.Title == command.Title),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when price is invalid.
    /// </summary>
    [Fact(DisplayName = "Invalid price should throw ValidationException")]
    public async Task Handle_InvalidPrice_ThrowsValidationException()
    {
        // Arrange
        var command = CreateProductHandlerTestData.GenerateCommandWithInvalidPrice();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository was not called
        await _productRepository.DidNotReceive().AddAsync(Arg.Any<Product>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler uses validation from the Product entity.
    /// </summary>
    [Fact(DisplayName = "Product validation is used")]
    public async Task Handle_ProductValidationIsUsed()
    {
        // Arrange: A command with invalid price should trigger validation
        var command = CreateProductHandlerTestData.GenerateCommandWithInvalidPrice();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository was not called
        await _productRepository.DidNotReceive().AddAsync(Arg.Any<Product>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<ProductCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}