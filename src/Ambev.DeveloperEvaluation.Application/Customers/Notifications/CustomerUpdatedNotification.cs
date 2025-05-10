using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Notifications;

/// <summary>
/// Notification for customer update events.
/// </summary>
public class CustomerUpdatedNotification : INotification
{
    /// <summary>
    /// The identifier of the updated customer.
    /// </summary>
    public string Id { get; set; }
}
