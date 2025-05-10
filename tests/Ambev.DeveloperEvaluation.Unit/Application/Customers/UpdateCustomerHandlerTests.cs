using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Customers;

/// <summary>
/// Contains unit tests for the UpdateCustomerHandler class.
/// </summary>
public class UpdateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UpdateCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateCustomerHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public UpdateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();

        _handler = new UpdateCustomerHandler(
            _customerRepository,
            _mediator,
            _mapper
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when customer is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent customer should throw NotFoundException")]
    public async Task Handle_NonExistentCustomer_ThrowsNotFoundException()
    {
        // Arrange
        var command = UpdateCustomerHandlerTestData.GenerateValidCommand();

        _customerRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Customer with ID {command.Id} not found.");

        // Verify repository calls
        await _customerRepository.Received(1).GetByIdAsync(command.Id);
        await _customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Customer>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerUpdatedNotification>(),
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
        var command = UpdateCustomerHandlerTestData.GenerateCommandWithInvalidEmail();
        var customer = CustomerTestData.GenerateCustomerWithId(command.Id);

        _customerRepository.GetByIdAsync(command.Id).Returns(customer);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository calls
        await _customerRepository.Received(1).GetByIdAsync(command.Id);
        await _customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Customer>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}