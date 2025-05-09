namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Result of branch creation operation
/// </summary>
public class CreateBranchResult
{
    /// <summary>
    /// ID of the created branch
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the created branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// City of the created branch
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State of the created branch
    /// </summary>
    public string State { get; set; }
}
