using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Request model for updating a branch
/// </summary>
public class UpdateBranchRequest
{
    /// <summary>
    /// Updated name of the branch
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Updated address of the branch
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// Updated city of the branch
    /// </summary>
    [Required]
    public string City { get; set; }

    /// <summary>
    /// Updated state of the branch
    /// </summary>
    [Required]
    public string State { get; set; }

    /// <summary>
    /// Updated ZIP code of the branch
    /// </summary>
    [Required]
    public string ZipCode { get; set; }

    /// <summary>
    /// Updated phone number of the branch
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Updated email of the branch
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }
}
