using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;

/// <summary>
/// Query to get a branch by ID
/// </summary>
public class GetBranchByIdQuery : IRequest<BranchDetailVm>
{
    /// <summary>
    /// ID of the branch to get
    /// </summary>
    public Guid Id { get; set; }
}
