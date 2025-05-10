using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranchById;

/// <summary>
/// Validator for the get branch by ID query
/// </summary>
public class GetBranchByIdValidator : AbstractValidator<GetBranchByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetBranchByIdValidator
    /// </summary>
    public GetBranchByIdValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Valid branch ID is required.");
    }
}
