using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

/// <summary>
/// Validator for the get customer by ID query
/// </summary>
public class GetCustomerByIdValidator : AbstractValidator<GetCustomerByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetCustomerByIdValidator
    /// </summary>
    public GetCustomerByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
