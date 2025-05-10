using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;

/// <summary>
/// Handler for deactivating a branch
/// </summary>
public class DeactivateBranchHandler : IRequestHandler<DeactivateBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the DeactivateBranchHandler
    /// </summary>
    public DeactivateBranchHandler(IBranchRepository branchRepository, IMediator mediator)
    {
        _branchRepository = branchRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the deactivate branch command
    /// </summary>
    public async Task<bool> Handle(DeactivateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        var result = await _branchRepository.DeactivateAsync(request.Id);

        if (result)
        {
            await _mediator.Publish(new BranchDeactivatedNotification
            {
                Id = branch.Id,
                Name = branch.Name
            }, cancellationToken);
        }

        return result;
    }
}