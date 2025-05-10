using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches;

/// <summary>
/// Contains unit tests for the CreateBranchHandler class.
/// </summary>
public class CreateBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;
    private readonly CreateBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateBranchHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CreateBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mediator = Substitute.For<IMediator>();

        _handler = new CreateBranchHandler(
            _branchRepository,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid create branch command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should create branch successfully")]
    public async Task Handle_ValidCommand_CreatesBranchSuccessfully()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateValidCommand();

        _branchRepository.AddAsync(Arg.Any<Branch>()).Returns(callInfo =>
        {
            var branch = callInfo.Arg<Branch>();
            return branch;
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.City.Should().Be(command.City);
        result.State.Should().Be(command.State);

        // Verify repository was called
        await _branchRepository.Received(1).AddAsync(Arg.Is<Branch>(b =>
            b.Name == command.Name &&
            b.Address == command.Address &&
            b.City == command.City &&
            b.State == command.State &&
            b.ZipCode == command.ZipCode &&
            b.Phone == command.Phone &&
            b.Email == command.Email
        ));

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<BranchCreatedNotification>(n => n.Name == command.Name),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when validation fails.
    /// </summary>
    [Fact(DisplayName = "Invalid email should throw ValidationException")]
    public async Task Handle_InvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateCommandWithInvalidEmail();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository was not called
        await _branchRepository.DidNotReceive().AddAsync(Arg.Any<Branch>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when name is empty.
    /// </summary>
    [Fact(DisplayName = "Empty name should throw ValidationException")]
    public async Task Handle_EmptyName_ThrowsValidationException()
    {
        // Arrange
        var command = CreateBranchHandlerTestData.GenerateCommandWithEmptyName();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        // Verify repository was not called
        await _branchRepository.DidNotReceive().AddAsync(Arg.Any<Branch>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchCreatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}