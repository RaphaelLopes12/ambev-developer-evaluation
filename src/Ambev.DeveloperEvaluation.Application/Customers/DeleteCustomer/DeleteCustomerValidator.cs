using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

/// <summary>
/// Validator for the delete customer command
/// </summary>
public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteCustomerValidator
    /// </summary>
    public DeleteCustomerValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
