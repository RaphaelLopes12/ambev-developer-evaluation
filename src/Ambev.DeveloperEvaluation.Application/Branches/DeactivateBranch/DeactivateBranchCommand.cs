using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;

/// <summary>
/// Command to deactivate a branch
/// </summary>
public class DeactivateBranchCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the branch to deactivate
    /// </summary>
    public Guid Id { get; set; }
}
