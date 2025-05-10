using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

/// <summary>
/// Exception thrown when domain validation fails
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Validation errors
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationException with a simple message
    /// </summary>
    /// <param name="message">Error message</param>
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>
        {
            { "Error", new[] { message } }
        };
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    /// <param name="errors">Validation errors</param>
    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    public ValidationException(ValidationResultDetail validationErrors)
        : base("One or more validation failures have occurred.")
    {
        Errors = validationErrors.Errors
            .GroupBy(x => x.Error, x => x.Detail)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException from FluentValidation errors
    /// </summary>
    /// <param name="validationFailures">FluentValidation errors</param>
    public ValidationException(IEnumerable<ValidationFailure> validationFailures)
        : base("One or more validation failures have occurred.")
    {
        Errors = validationFailures
            .GroupBy(x => x.ErrorCode, x => x.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}