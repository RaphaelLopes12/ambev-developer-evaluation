using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Handler for updating a branch
/// </summary>
public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the UpdateBranchHandler
    /// </summary>
    public UpdateBranchHandler(IBranchRepository branchRepository, IMediator mediator)
    {
        _branchRepository = branchRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the update branch command
    /// </summary>
    public async Task<UpdateBranchResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        var result = await _branchRepository.UpdateDetailsAsync(
            request.Id,
            request.Name,
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.Phone,
            request.Email
        );

        if (!result)
        {
            throw new AppException($"Failed to update branch with ID {request.Id}.");
        }

        var updatedBranch = await _branchRepository.GetByIdAsync(request.Id);

        await _mediator.Publish(new BranchUpdatedNotification
        {
            Id = updatedBranch.Id,
            Name = updatedBranch.Name
        }, cancellationToken);

        return new UpdateBranchResult
        {
            Id = updatedBranch.Id,
            Name = updatedBranch.Name,
            City = updatedBranch.City,
            State = updatedBranch.State
        };
    }
}