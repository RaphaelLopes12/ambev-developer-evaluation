namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

/// <summary>
/// Response model for branch list
/// </summary>
public class GetBranchesResponse
{
    /// <summary>
    /// List of branches
    /// </summary>
    public List<BranchListItem> Branches { get; set; } = new List<BranchListItem>();
}
