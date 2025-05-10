namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranches;

/// <summary>
/// Branch list item information
/// </summary>
public class BranchListItem
{
    /// <summary>
    /// ID of the branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// City of the branch
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State of the branch
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Branch status (active/inactive)
    /// </summary>
    public bool IsActive { get; set; }
}
