namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Response model for branch details
/// </summary>
public class GetBranchResponse
{
    /// <summary>
    /// ID of the branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Address of the branch
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// City of the branch
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State of the branch
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// ZIP code of the branch
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// Phone number of the branch
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Email of the branch
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Branch status (active/inactive)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
