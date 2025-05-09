namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetActiveBranches;

/// <summary>
/// Response model for active branch list
/// </summary>
public class GetActiveBranchesResponse
{
    /// <summary>
    /// List of active branches
    /// </summary>
    public List<ActiveBranchItem> Branches { get; set; } = new List<ActiveBranchItem>();
}
