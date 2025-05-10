using Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches;

/// <summary>
/// Contains unit tests for the ActivateBranchHandler class.
/// </summary>
public class ActivateBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;
    private readonly ActivateBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the ActivateBranchHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public ActivateBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mediator = Substitute.For<IMediator>();

        _handler = new ActivateBranchHandler(
            _branchRepository,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid activate branch command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should activate branch successfully")]
    public async Task Handle_ValidCommand_ActivatesBranchSuccessfully()
    {
        // Arrange
        var command = ActivateBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateInactiveBranch();
        typeof(Branch).GetProperty("Id").SetValue(branch, command.Id);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch);
        _branchRepository.ActivateAsync(command.Id).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).ActivateAsync(command.Id);

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<BranchActivatedNotification>(n =>
                n.Id == command.Id &&
                n.Name == branch.Name),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when branch is not found.
    /// </summary>
    [Fact(DisplayName = "Non-existent branch should throw NotFoundException")]
    public async Task Handle_NonExistentBranch_ThrowsNotFoundException()
    {
        // Arrange
        var command = ActivateBranchHandlerTestData.GenerateValidCommand();

        _branchRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Branch with ID {command.Id} not found.");

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.DidNotReceive().ActivateAsync(Arg.Any<Guid>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchActivatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when activation fails.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = ActivateBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateInactiveBranch();
        typeof(Branch).GetProperty("Id").SetValue(branch, command.Id);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch);
        _branchRepository.ActivateAsync(command.Id).Returns(Task.FromResult(false));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).ActivateAsync(command.Id);

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchActivatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}