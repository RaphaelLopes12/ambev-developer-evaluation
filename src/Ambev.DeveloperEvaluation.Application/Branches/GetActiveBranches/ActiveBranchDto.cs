namespace Ambev.DeveloperEvaluation.Application.Branches.GetActiveBranches;

/// <summary>
/// Data transfer object for active branch information
/// </summary>
public class ActiveBranchDto
{
    /// <summary>
    /// Branch ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Branch name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// City and state where the branch is located
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Contact information (phone and email)
    /// </summary>
    public string Contact { get; set; }
}
