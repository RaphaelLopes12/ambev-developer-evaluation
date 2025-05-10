using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
/// Contains unit tests for the UpdateBranchHandler class.
/// </summary>
public class UpdateBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;
    private readonly UpdateBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateBranchHandlerTests class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public UpdateBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mediator = Substitute.For<IMediator>();

        _handler = new UpdateBranchHandler(
            _branchRepository,
            _mediator
        );
    }

    /// <summary>
    /// Tests that a valid update branch command is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Valid command should update branch successfully")]
    public async Task Handle_ValidCommand_UpdatesBranchSuccessfully()
    {
        // Arrange
        var command = UpdateBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateBranchWithId(command.Id);
        var updatedBranch = BranchTestData.GenerateBranchWithId(command.Id);

        // Set updatedBranch properties to match command
        typeof(Branch).GetProperty("Name").SetValue(updatedBranch, command.Name);
        typeof(Branch).GetProperty("City").SetValue(updatedBranch, command.City);
        typeof(Branch).GetProperty("State").SetValue(updatedBranch, command.State);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch, updatedBranch);
        _branchRepository.UpdateDetailsAsync(
            command.Id,
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.ZipCode,
            command.Phone,
            command.Email
        ).Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.City.Should().Be(command.City);
        result.State.Should().Be(command.State);

        // Verify repository calls
        await _branchRepository.Received(2).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).UpdateDetailsAsync(
            command.Id,
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.ZipCode,
            command.Phone,
            command.Email
        );

        // Verify notification was published
        await _mediator.Received(1).Publish(
            Arg.Is<BranchUpdatedNotification>(n =>
                n.Id == command.Id &&
                n.Name == command.Name),
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
        var command = UpdateBranchHandlerTestData.GenerateValidCommand();

        _branchRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Branch with ID {command.Id} not found.");

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.DidNotReceive().UpdateDetailsAsync(
            Arg.Any<Guid>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the handler throws an exception when repository update fails.
    /// </summary>
    [Fact(DisplayName = "Repository failure should throw AppException")]
    public async Task Handle_RepositoryFailure_ThrowsAppException()
    {
        // Arrange
        var command = UpdateBranchHandlerTestData.GenerateValidCommand();
        var branch = BranchTestData.GenerateBranchWithId(command.Id);

        _branchRepository.GetByIdAsync(command.Id).Returns(branch);
        _branchRepository.UpdateDetailsAsync(
            command.Id,
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.ZipCode,
            command.Phone,
            command.Email
        ).Returns(Task.FromResult(false));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AppException>()
            .WithMessage($"Failed to update branch with ID {command.Id}.");

        // Verify repository calls
        await _branchRepository.Received(1).GetByIdAsync(command.Id);
        await _branchRepository.Received(1).UpdateDetailsAsync(
            command.Id,
            command.Name,
            command.Address,
            command.City,
            command.State,
            command.ZipCode,
            command.Phone,
            command.Email
        );

        // Verify no notification was published
        await _mediator.DidNotReceive().Publish(
            Arg.Any<BranchUpdatedNotification>(),
            Arg.Any<CancellationToken>()
        );
    }
}