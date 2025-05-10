namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// Response model for branch deletion
/// </summary>
public class DeleteBranchResponse
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result
    /// </summary>
    public string Message { get; set; }
}
