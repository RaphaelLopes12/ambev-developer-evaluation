namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetActiveBranches;

/// <summary>
/// Active branch list item information
/// </summary>
public class ActiveBranchItem
{
    /// <summary>
    /// ID of the branch
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location (City, State)
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Contact information (Phone and Email)
    /// </summary>
    public string Contact { get; set; }
}
