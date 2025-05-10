using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches;

/// <summary>
/// Contains unit tests for the DeleteBranchHandler class.
/// </summary>
public class DeleteBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;
    private readonly DeleteBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the DeleteBranchHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public DeleteBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mediator = Substitute.For<IMediator>();

        _handler = new DeleteBranchHandler(
            _branchRepository,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid delete branch command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should delete branch successfully")]
    public async Task Handle_ValidCommand_DeletesBranchSuccessfully()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateBranchWithId(command.Id);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch);
        _branchRepository.RemoveAsync(command.Id).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).RemoveAsync(command.Id);

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<BranchDeletedNotification>(n =>
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
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();

        _branchRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Branch with ID {command.Id} not found.");

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.DidNotReceive().RemoveAsync(Arg.Any<Guid>());

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler returns false when removal fails.
    /// </summary>
    [Fact(DisplayName = "Repository failure should return false")]
    public async Task Handle_RepositoryFailure_ReturnsFalse()
    {
        // Arrange
        var command = DeleteBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateBranchWithId(command.Id);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch);
        _branchRepository.RemoveAsync(command.Id).Returns(Task.FromResult(false));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).RemoveAsync(command.Id);

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchDeletedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}