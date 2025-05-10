namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ActivateBranch;

/// <summary>
/// Response model for branch activation
/// </summary>
public class ActivateBranchResponse
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
