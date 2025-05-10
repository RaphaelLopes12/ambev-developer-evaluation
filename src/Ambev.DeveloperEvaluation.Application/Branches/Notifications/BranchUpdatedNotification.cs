using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Notification for branch update events.
/// </summary>
public class BranchUpdatedNotification : INotification
{
    /// <summary>
    /// The identifier of the updated branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the updated branch.
    /// </summary>
    public string Name { get; set; }
}
