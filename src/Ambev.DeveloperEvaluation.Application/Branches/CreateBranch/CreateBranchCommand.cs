using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Command to create a new branch
/// </summary>
public class CreateBranchCommand : IRequest<CreateBranchResult>
{
    /// <summary>
    /// Name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Address of the branch
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
}
