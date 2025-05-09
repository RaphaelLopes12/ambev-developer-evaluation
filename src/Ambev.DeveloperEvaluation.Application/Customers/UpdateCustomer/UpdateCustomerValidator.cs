using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;

/// <summary>
/// Validator for the update customer command
/// </summary>
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateCustomerValidator
    /// </summary>
    public UpdateCustomerValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Customer ID is required.");

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
