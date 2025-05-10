using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.Notifications;

/// <summary>
/// Notification for branch activation events.
/// </summary>
public class BranchActivatedNotification : INotification
{
    /// <summary>
    /// The identifier of the activated branch.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the activated branch.
    /// </summary>
    public string Name { get; set; }
}
