using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;

/// <summary>
/// Handler for activating a branch
/// </summary>
public class ActivateBranchHandler : IRequestHandler<ActivateBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of the ActivateBranchHandler
    /// </summary>
    public ActivateBranchHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
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

        return await _branchRepository.ActivateAsync(request.Id);
    }
}
