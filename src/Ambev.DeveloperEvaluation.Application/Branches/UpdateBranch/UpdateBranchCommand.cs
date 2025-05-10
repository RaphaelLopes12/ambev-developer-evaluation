using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Command to update an existing branch
/// </summary>
public class UpdateBranchCommand : IRequest<UpdateBranchResult>
{
    /// <summary>
    /// ID of the branch to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated name of the branch
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Updated address of the branch
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Updated city where the branch is located
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Updated state where the branch is located
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Updated ZIP code of the branch
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// Updated phone number of the branch
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Updated email of the branch
    /// </summary>
    public string Email { get; set; }
}
