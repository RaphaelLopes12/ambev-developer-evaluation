using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Notification for branch deactivation events.
/// </summary>
public class BranchDeactivatedNotification : INotification
{
    /// <summary>
    /// The identifier of the deactivated branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the deactivated branch.
    /// </summary>
    public string Name { get; set; }
}
