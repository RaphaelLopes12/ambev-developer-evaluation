using Ambev.DeveloperEvaluation.Application.Branches.Notifications;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for deleting a branch
/// </summary>
public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the DeleteBranchHandler
    /// </summary>
    public DeleteBranchHandler(IBranchRepository branchRepository, IMediator mediator)
    {
        _branchRepository = branchRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the delete branch command
    /// </summary>
    public async Task<bool> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        var branchId = branch.Id;
        var branchName = branch.Name;

        var result = await _branchRepository.RemoveAsync(request.Id);

        if (result)
        {
            await _mediator.Publish(new BranchDeletedNotification
            {
                Id = branchId,
                Name = branchName
            }, cancellationToken);
        }

        return result;
    }
}