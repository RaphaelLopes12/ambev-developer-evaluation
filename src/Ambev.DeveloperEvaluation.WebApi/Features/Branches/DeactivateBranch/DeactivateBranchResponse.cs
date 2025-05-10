namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeactivateBranch;

/// <summary>
/// Response model for branch deactivation
/// </summary>
public class DeactivateBranchResponse
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
