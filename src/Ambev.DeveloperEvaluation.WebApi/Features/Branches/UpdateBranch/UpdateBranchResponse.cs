namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Response model for branch update
/// </summary>
public class UpdateBranchResponse
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result
    /// </summary>
    public string Message { get; set; }

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
