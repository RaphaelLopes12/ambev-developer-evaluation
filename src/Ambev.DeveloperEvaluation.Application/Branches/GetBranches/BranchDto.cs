namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranches;

/// <summary>
/// Data transfer object for branch information
/// </summary>
public class BranchDto
{
    /// <summary>
    /// Branch ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Branch name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Branch address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// City where the branch is located
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State where the branch is located
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Branch status (active/inactive)
    /// </summary>
    public bool IsActive { get; set; }
}
