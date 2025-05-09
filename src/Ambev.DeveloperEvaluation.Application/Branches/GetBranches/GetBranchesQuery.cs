using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Query to get all branches
/// </summary>
public class GetBranchesQuery : IRequest<List<BranchDto>>
{
}
