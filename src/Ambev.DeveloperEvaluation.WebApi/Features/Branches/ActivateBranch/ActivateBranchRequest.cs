using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ActivateBranch;

/// <summary>
/// Request model for activating a branch
/// </summary>
public class ActivateBranchRequest
{
    /// <summary>
    /// ID of the branch to activate
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}
