using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Notifications;

/// <summary>
/// Notification for customer deletion events.
/// </summary>
public class CustomerDeletedNotification : INotification
{
    /// <summary>
    /// The identifier of the deleted customer.
    /// </summary>
    public string Id { get; set; }
}
