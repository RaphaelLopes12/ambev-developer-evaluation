namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Result of branch update operation
/// </summary>
public class UpdateBranchResult
{
    /// <summary>
    /// ID of the updated branch
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Updated name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated city of the branch
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Updated state of the branch
    /// </summary>
    public string State { get; set; }
}
