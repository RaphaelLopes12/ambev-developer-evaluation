using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Customers;

/// <summary>
/// Contains unit tests for the DeleteCustomerHandler class.
/// </summary>
public class DeleteCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly DeleteCustomerHandler _handler;

    /// <summary>
    /// Initializes a new instance of the DeleteCustomerHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public DeleteCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mediator = Substitute.For<IMediator>();

        _handler = new DeleteCustomerHandler(
            _customerRepository,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid delete customer command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should delete customer successfully")]
    public async Task Handle_ValidCommand_DeletesCustomerSuccessfully()
    {
        // Arrange
        var command = DeleteCustomerHandlerTestData.GenerateValidCommand();
        var customer = CustomerTestData.GenerateCustomerWithId(command.Id);

        _customerRepository.GetByIdAsync(command.Id).Returns(customer);
        _customerRepository.RemoveAsync(command.Id).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _customerRepository.Received(1).GetByIdAsync(command.Id);
        await _customerRepository.Received(1).RemoveAsync(command.Id);

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<CustomerDeletedNotification>(n => n.Id == command.Id),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when customer is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent customer should throw NotFoundException")]
    public async Task Handle_NonExistentCustomer_ThrowsNotFoundException()
    {
        // Arrange
        var command = DeleteCustomerHandlerTestData.GenerateValidCommand();

        _customerRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Customer with ID {command.Id} not found.");

        // Verify repository calls
        await _customerRepository.Received(1).GetByIdAsync(command.Id);
        await _customerRepository.DidNotReceive().RemoveAsync(Arg.Any<string>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when repository fails to delete.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = DeleteCustomerHandlerTestData.GenerateValidCommand();
        var customer = CustomerTestData.GenerateCustomerWithId(command.Id);

        _customerRepository.GetByIdAsync(command.Id).Returns(customer);
        _customerRepository.RemoveAsync(command.Id).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _customerRepository.Received(1).GetByIdAsync(command.Id);
        await _customerRepository.Received(1).RemoveAsync(command.Id);

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<CustomerDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}
