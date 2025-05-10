using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Customers;

/// <summary>
/// Contains unit tests for the CreateCustomerHandler class.
/// </summary>
public class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly CreateCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateCustomerHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CreateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();

        _handler = new CreateCustomerHandler(
            _customerRepository,
            _mediator,
            _mapper
        );
    }

    /// <summary>
    /// Tests that a valid create customer command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should create customer successfully")]
    public async Task Handle_ValidCommand_CreatesCustomerSuccessfully()
    {
        // Arrange
        var command = CreateCustomerHandlerTestData.GenerateValidCommand();

        _customerRepository.AddAsync(Arg.Any<Customer>()).Returns(callInfo =>
        {
            var customer = callInfo.Arg<Customer>();
            // Set a MongoDB ID for the created customer
            typeof(Customer).GetProperty("Id").SetValue(customer, MongoDB.Bson.ObjectId.GenerateNewId().ToString());
            return customer;
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);

        await _customerRepository.Received(1).AddAsync(Arg.Is<Customer>(c =>
            c.Name == command.Name &&
            c.Email == command.Email &&
            c.Phone == command.Phone &&
            c.Address == command.Address
        ));

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<CustomerCreatedNotification>(n => n.Id != null),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when validation fails.
    /// </summary>
    [Fact(DisplayName = "Invalid customer should throw ValidationException")]
    public async Task Handle_InvalidCustomer_ThrowsValidationException()
    {
        // Arrange
        var command = CreateCustomerHandlerTestData.GenerateCommandWithInvalidEmail();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _customerRepository.DidNotReceive().AddAsync(Arg.Any<Customer>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when the name is empty.
    /// </summary>
    [Fact(DisplayName = "Empty name should throw ValidationException")]
    public async Task Handle_EmptyName_ThrowsValidationException()
    {
        // Arrange
        var command = CreateCustomerHandlerTestData.GenerateCommandWithEmptyName();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _customerRepository.DidNotReceive().AddAsync(Arg.Any<Customer>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}