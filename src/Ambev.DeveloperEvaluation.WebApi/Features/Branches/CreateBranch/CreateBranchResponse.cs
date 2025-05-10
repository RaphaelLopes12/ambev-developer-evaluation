namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Response model for branch creation
/// </summary>
public class CreateBranchResponse
{
    /// <summary>
    /// ID of the created branch
    /// </summary>
    public Guid Id { get; set; }

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
