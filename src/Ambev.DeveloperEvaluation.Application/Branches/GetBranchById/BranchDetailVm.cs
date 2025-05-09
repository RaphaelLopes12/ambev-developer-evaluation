namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;

/// <summary>
/// View model for detailed branch information
/// </summary>
public class BranchDetailVm
{
    /// <summary>
    /// Branch ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Branch name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Branch address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// City where the branch is located
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// State where the branch is located
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
