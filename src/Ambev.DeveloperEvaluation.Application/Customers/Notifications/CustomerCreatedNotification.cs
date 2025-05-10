using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.Notifications;

/// <summary>
/// Notification for customer creation events.
/// </summary>
public class CustomerCreatedNotification : INotification
{
    /// <summary>
    /// The identifier of the created customer.
    /// </summary>
    public string Id { get; set; }
}
