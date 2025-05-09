using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a company branch.
/// </summary>
public class Branch
{
    /// <summary>
    /// Unique identifier of the branch.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Name of the branch.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Complete address of the branch.
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// City where the branch is located.
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// State where the branch is located.
    /// </summary>
    public string State { get; private set; }

    /// <summary>
    /// Zip code of the branch.
    /// </summary>
    public string ZipCode { get; private set; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    /// Contact email.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Indicates if the branch is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Sales made at this branch.
    /// </summary>
    public virtual ICollection<Sale> Sales { get; private set; }

    /// <summary>
    /// Required for EF.
    /// </summary>
    public Branch() { }

    /// <summary>
    /// Creates a new branch with the specified details.
    /// </summary>
    public Branch(string name, string address, string city, string state, string zipCode, string phone, string email)
    {
        Name = name;
        Address = address;
        City = city;
        State = state;
        ZipCode = zipCode;
        Phone = phone;
        Email = email;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Sales = new List<Sale>();
    }

    /// <summary>
    /// Updates the branch details.
    /// </summary>
    public void UpdateDetails(string name, string address, string city, string state, string zipCode, string phone, string email)
    {
        Name = name;
        Address = address;
        City = city;
        State = state;
        ZipCode = zipCode;
        Phone = phone;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the branch.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Branch is already active.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the branch.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Branch is already inactive.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the current branch state.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new BranchValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
}

/// <summary>
/// Validator for Branch entity.
/// </summary>
public class BranchValidator : AbstractValidator<Branch>
{
    public BranchValidator()
    {
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
            .NotEmpty().WithMessage("Zip code is required.")
            .MaximumLength(20).WithMessage("Zip code cannot exceed 20 characters.");

        RuleFor(b => b.Email)
            .EmailAddress().When(b => !string.IsNullOrEmpty(b.Email))
            .WithMessage("A valid email address is required.");

        RuleFor(b => b.Phone)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
    }
}