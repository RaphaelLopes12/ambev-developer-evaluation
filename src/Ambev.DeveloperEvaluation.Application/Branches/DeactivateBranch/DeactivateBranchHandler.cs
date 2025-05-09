using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;

/// <summary>
/// Handler for deactivating a branch
/// </summary>
public class DeactivateBranchHandler : IRequestHandler<DeactivateBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of the DeactivateBranchHandler
    /// </summary>
    public DeactivateBranchHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
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

        return await _branchRepository.DeactivateAsync(request.Id);
    }
}
