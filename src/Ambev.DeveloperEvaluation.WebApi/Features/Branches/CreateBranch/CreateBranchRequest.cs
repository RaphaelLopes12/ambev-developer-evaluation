using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Request model for creating a branch
/// </summary>
public class CreateBranchRequest
{
    /// <summary>
    /// Name of the branch
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Address of the branch
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// City of the branch
    /// </summary>
    [Required]
    public string City { get; set; }

    /// <summary>
    /// State of the branch
    /// </summary>
    [Required]
    public string State { get; set; }

    /// <summary>
    /// ZIP code of the branch
    /// </summary>
    [Required]
    public string ZipCode { get; set; }

    /// <summary>
    /// Phone number of the branch
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Email of the branch
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }
}
