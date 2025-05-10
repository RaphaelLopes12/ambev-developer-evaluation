using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Notification for branch creation events.
/// </summary>
public class BranchCreatedNotification : INotification
{
    /// <summary>
    /// The identifier of the created branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the created branch.
    /// </summary>
    public string Name { get; set; }
}
