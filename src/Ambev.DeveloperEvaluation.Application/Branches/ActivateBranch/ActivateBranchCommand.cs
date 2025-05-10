using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;

/// <summary>
/// Command to activate a branch
/// </summary>
public class ActivateBranchCommand : IRequest<bool>
{
    /// <summary>
    /// ID of the branch to activate
    /// </summary>
    public Guid Id { get; set; }
}
