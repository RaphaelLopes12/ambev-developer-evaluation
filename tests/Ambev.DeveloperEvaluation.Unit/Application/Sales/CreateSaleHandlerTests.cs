using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Application.Sales.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateSaleHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();

        _handler = new CreateSaleHandler(
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
    /// Tests that a valid sale creation command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should create sale successfully")]
    public async Task Handle_ValidCommand_CreatesSaleSuccessfully()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // Create customer with proper constructor
        var customer = new Customer(
            name: "Test Customer",
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        // Set Id using reflection
        SetPrivateProperty(customer, "Id", command.CustomerId);

        // Create branch with proper constructor
        var branch = new Branch(
            name: "Test Branch",
            address: "Branch Address",
            city: "Test City",
            state: "Test State",
            zipCode: "12345",
            phone: "+0987654321",
            email: "branch@example.com"
        );
        // Set Id using reflection
        SetPrivateProperty(branch, "Id", command.BranchId);

        // Setup mocks for all products in the command
        foreach (var item in command.Items)
        {
            var product = new Product(
                title: item.ProductName,
                price: item.UnitPrice,
                description: "Test Description",
                category: "Test Category",
                image: "test-image.jpg",
                stockQuantity: item.Quantity * 2
            );
            // Set Id using reflection
            SetPrivateProperty(product, "Id", item.ProductId);

            _productRepository.GetByIdAsync(item.ProductId).Returns(product);
        }

        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).Returns(branch);

        var createdSale = new Sale(
            command.Number,
            command.Date,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        var expectedResult = new CreateSaleResult
        {
            Id = createdSale.Id
        };

        _mapper.Map<CreateSaleResult>(createdSale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResult);

        // Verify repository calls
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        // Verify each product's stock was updated
        foreach (var item in command.Items)
        {
            await _productRepository.Received(1).UpdateStockAsync(
                item.ProductId,
                Arg.Any<int>()
            );
        }

        // Verify notification was published
        await _mediator.Received(1).Publish(Arg.Is<SaleCreatedNotification>(
            n => n.SaleId == createdSale.Id),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when customer is not found.
    /// </summary>
    [Fact(DisplayName = "Command with non-existent customer should throw NotFoundException")]
    public async Task Handle_NonExistentCustomer_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        _customerRepository.GetByIdAsync(command.CustomerId).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Customer with ID {command.CustomerId} not found*");
    }

    /// <summary>
    /// Tests that the handler throws an exception when branch is not found.
    /// </summary>
    [Fact(DisplayName = "Command with non-existent branch should throw NotFoundException")]
    public async Task Handle_NonExistentBranch_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // Create customer with proper constructor
        var customer = new Customer(
            name: "Test Customer",
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        // Set Id using reflection
        SetPrivateProperty(customer, "Id", command.CustomerId);

        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Branch with ID {command.BranchId} not found*");
    }

    /// <summary>
    /// Tests that the handler throws an exception when product is not found.
    /// </summary>
    [Fact(DisplayName = "Command with non-existent product should throw NotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand(1);

        // Create customer with proper constructor
        var customer = new Customer(
            name: "Test Customer",
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        // Set Id using reflection
        SetPrivateProperty(customer, "Id", command.CustomerId);

        // Create branch with proper constructor
        var branch = new Branch(
            name: "Test Branch",
            address: "Branch Address",
            city: "Test City",
            state: "Test State",
            zipCode: "12345",
            phone: "+0987654321",
            email: "branch@example.com"
        );
        // Set Id using reflection
        SetPrivateProperty(branch, "Id", command.BranchId);

        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).Returns(branch);
        _productRepository.GetByIdAsync(command.Items[0].ProductId).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Product with ID {command.Items[0].ProductId} not found*");
    }

    /// <summary>
    /// Tests that the handler throws an exception when product stock is insufficient.
    /// </summary>
    [Fact(DisplayName = "Command with insufficient stock should throw DomainException")]
    public async Task Handle_InsufficientStock_ThrowsDomainException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand(1);

        // Create customer with proper constructor
        var customer = new Customer(
            name: "Test Customer",
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        // Set Id using reflection
        SetPrivateProperty(customer, "Id", command.CustomerId);

        // Create branch with proper constructor
        var branch = new Branch(
            name: "Test Branch",
            address: "Branch Address",
            city: "Test City",
            state: "Test State",
            zipCode: "12345",
            phone: "+0987654321",
            email: "branch@example.com"
        );
        // Set Id using reflection
        SetPrivateProperty(branch, "Id", command.BranchId);

        // Create product with insufficient stock
        var product = new Product(
            title: command.Items[0].ProductName,
            price: command.Items[0].UnitPrice,
            description: "Test Description",
            category: "Test Category",
            image: "test-image.jpg",
            stockQuantity: command.Items[0].Quantity - 1
        );
        // Set Id using reflection
        SetPrivateProperty(product, "Id", command.Items[0].ProductId);

        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).Returns(branch);
        _productRepository.GetByIdAsync(command.Items[0].ProductId).Returns(product);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"*Insufficient stock for product*");
    }

    /// <summary>
    /// Tests that the handler handles repository exceptions.
    /// </summary>
    [Fact(DisplayName = "Repository exception should be propagated")]
    public async Task Handle_RepositoryException_ThrowsException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // Create customer with proper constructor
        var customer = new Customer(
            name: "Test Customer",
            email: "test@example.com",
            phone: "+1234567890",
            address: "Test Address"
        );
        // Set Id using reflection
        SetPrivateProperty(customer, "Id", command.CustomerId);

        // Create branch with proper constructor
        var branch = new Branch(
            name: "Test Branch",
            address: "Branch Address",
            city: "Test City",
            state: "Test State",
            zipCode: "12345",
            phone: "+0987654321",
            email: "branch@example.com"
        );
        // Set Id using reflection
        SetPrivateProperty(branch, "Id", command.BranchId);

        // Setup mocks for all products in the command
        foreach (var item in command.Items)
        {
            var product = new Product(
                title: item.ProductName,
                price: item.UnitPrice,
                description: "Test Description",
                category: "Test Category",
                image: "test-image.jpg",
                stockQuantity: item.Quantity * 2
            );
            // Set Id using reflection
            SetPrivateProperty(product, "Id", item.ProductId);

            _productRepository.GetByIdAsync(item.ProductId).Returns(product);
        }

        _customerRepository.GetByIdAsync(command.CustomerId).Returns(customer);
        _branchRepository.GetByIdAsync(command.BranchId).Returns(branch);

        var expectedException = new InvalidOperationException("Database error");
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Throws(expectedException);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
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