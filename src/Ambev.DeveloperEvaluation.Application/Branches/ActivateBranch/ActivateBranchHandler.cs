using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;

/// <summary>
/// Handler for activating a branch
/// </summary>
public class ActivateBranchHandler : IRequestHandler<ActivateBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the ActivateBranchHandler
    /// </summary>
    public ActivateBranchHandler(IBranchRepository branchRepository, IMediator mediator)
    {
        _branchRepository = branchRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the activate branch command
    /// </summary>
    public async Task<bool> Handle(ActivateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        var result = await _branchRepository.ActivateAsync(request.Id);

        if (result)
        {
            await _mediator.Publish(new BranchActivatedNotification
            {
                Id = branch.Id,
                Name = branch.Name
            }, cancellationToken);
        }

        return result;
    }
}