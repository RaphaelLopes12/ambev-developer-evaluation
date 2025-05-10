namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

/// <summary>
/// Base exception for application-specific exceptions
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ApplicationException
    /// </summary>
    public AppException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the ApplicationException with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public AppException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ApplicationException with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public AppException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
