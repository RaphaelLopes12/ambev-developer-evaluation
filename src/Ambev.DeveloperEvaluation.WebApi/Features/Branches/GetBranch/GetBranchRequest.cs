using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Request model for getting a branch by ID
/// </summary>
public class GetBranchRequest
{
    /// <summary>
    /// ID of the branch to get
    /// </summary>
    [Required]
    public int Id { get; set; }
}
