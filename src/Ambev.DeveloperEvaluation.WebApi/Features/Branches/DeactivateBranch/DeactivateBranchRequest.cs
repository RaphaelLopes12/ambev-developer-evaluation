using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeactivateBranch;

/// <summary>
/// Request model for deactivating a branch
/// </summary>
public class DeactivateBranchRequest
{
    /// <summary>
    /// ID of the branch to deactivate
    /// </summary>
    [Required]
    public int Id { get; set; }
}
