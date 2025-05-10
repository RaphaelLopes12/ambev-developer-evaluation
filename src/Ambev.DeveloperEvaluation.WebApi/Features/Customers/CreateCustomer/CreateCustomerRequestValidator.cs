using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;

/// <summary>
/// Validator for customer creation requests
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
    }
}
