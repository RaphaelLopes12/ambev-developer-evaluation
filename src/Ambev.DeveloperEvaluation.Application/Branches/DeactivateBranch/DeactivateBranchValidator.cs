using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeactivateBranch;

/// <summary>
/// Validator for the deactivate branch command
/// </summary>
public class DeactivateBranchValidator : AbstractValidator<DeactivateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeactivateBranchValidator
    /// </summary>
    public DeactivateBranchValidator()
    {
        RuleFor(b => b.Id)
            .GreaterThan(0).WithMessage("Valid branch ID is required.");
    }
}
