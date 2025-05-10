using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Command to delete a branch
/// </summary>
public class DeleteBranchCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the branch to delete
    /// </summary>
    public Guid Id { get; set; }
}
