using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Validator for the delete branch command
/// </summary>
public class DeleteBranchValidator : AbstractValidator<DeleteBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteBranchValidator
    /// </summary>
    public DeleteBranchValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Valid branch ID is required.");
    }
}
