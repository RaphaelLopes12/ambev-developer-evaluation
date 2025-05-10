using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// Request model for deleting a branch
/// </summary>
public class DeleteBranchRequest
{
    /// <summary>
    /// ID of the branch to delete
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}
