using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Notification for branch deletion events.
/// </summary>
public class BranchDeletedNotification : INotification
{
    /// <summary>
    /// The identifier of the deleted branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the deleted branch.
    /// </summary>
    public string Name { get; set; }
}
