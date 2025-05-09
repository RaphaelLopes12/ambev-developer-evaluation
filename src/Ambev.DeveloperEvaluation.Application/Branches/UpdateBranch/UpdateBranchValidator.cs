using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Validator for the update branch command
/// </summary>
public class UpdateBranchValidator : AbstractValidator<UpdateBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateBranchValidator
    /// </summary>
    public UpdateBranchValidator()
    {
        RuleFor(b => b.Id)
            .GreaterThan(0).WithMessage("Valid branch ID is required.");

        RuleFor(b => b.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(b => b.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

        RuleFor(b => b.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

        RuleFor(b => b.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State cannot exceed 50 characters.");

        RuleFor(b => b.ZipCode)
            .NotEmpty().WithMessage("ZIP code is required.")
            .MaximumLength(20).WithMessage("ZIP code cannot exceed 20 characters.");

        RuleFor(b => b.Email)
            .EmailAddress().When(b => !string.IsNullOrEmpty(b.Email))
            .WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(b => b.Phone)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
    }
}
