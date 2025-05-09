using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;

/// <summary>
/// Query to get all active branches
/// </summary>
public class GetActiveBranchesQuery : IRequest<List<ActiveBranchDto>>
{
}
