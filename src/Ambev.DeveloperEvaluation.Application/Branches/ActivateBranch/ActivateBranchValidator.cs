using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.ActivateBranch;

/// <summary>
/// Validator for the activate branch command
/// </summary>
public class ActivateBranchValidator : AbstractValidator<ActivateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the ActivateBranchValidator
    /// </summary>
    public ActivateBranchValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Valid branch ID is required.");
    }
}
