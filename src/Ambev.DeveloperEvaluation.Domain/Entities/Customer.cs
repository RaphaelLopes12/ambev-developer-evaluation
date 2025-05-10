using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a customer in the system.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier of the customer in MongoDB.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    /// <summary>
    /// Full name of the customer.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Contact email of the customer.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Phone number of the customer.
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    /// Shipping/billing address of the customer.
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// Creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Identifier of this customer in the PostgreSQL system (implementation of External Identities pattern).
    /// </summary>
    public int? PostgreSQLId { get; private set; }

    /// <summary>
    /// Required for MongoDB.
    /// </summary>
    public Customer() { }

    /// <summary>
    /// Creates a new customer with the specified details.
    /// </summary>
    public Customer(string name, string email, string phone, string address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the customer details.
    /// </summary>
    public void UpdateDetails(string name, string email, string phone, string address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the PostgreSQL identifier for this customer.
    /// </summary>
    public void SetPostgreSQLId(int id)
    {
        PostgreSQLId = id;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the current customer state.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new CustomerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
}

/// <summary>
/// Validator for Customer entity.
/// </summary>
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
    }
}