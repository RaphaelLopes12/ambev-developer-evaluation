using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for deleting a branch
/// </summary>
public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, bool>
{
    private readonly IBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of the DeleteBranchHandler
    /// </summary>
    public DeleteBranchHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
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

        return await _branchRepository.RemoveAsync(request.Id);
    }
}
