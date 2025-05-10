using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware;

/// <summary>
/// Middleware for handling and formatting exceptions in a consistent way
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the ExceptionHandlingMiddleware
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">The logger</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationEx)
        {
            await HandleValidationExceptionAsync(context, validationEx);
        }
        catch (NotFoundException notFoundEx)
        {
            await HandleNotFoundExceptionAsync(context, notFoundEx);
        }
        catch (DomainException domainEx)
        {
            await HandleDomainExceptionAsync(context, domainEx);
        }
        catch (Ambev.DeveloperEvaluation.Domain.Exceptions.AppException appEx)
        {
            await HandleApplicationExceptionAsync(context, appEx);
        }
        catch (Exception ex)
        {
            await HandleUnknownExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles validation exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="exception">The validation exception</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        _logger.LogWarning(exception, "Validation error occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = exception.Errors
            .SelectMany(error => error.Value.Select(detail => new ValidationErrorDetail
            {
                Error = error.Key,
                Detail = detail
            }));

        var response = new
        {
            Success = false,
            Message = "Validation failed",
            Errors = errors
        };

        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Handles not found exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="exception">The not found exception</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
    {
        _logger.LogWarning(exception, "Entity not found");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        var response = new
        {
            Success = false,
            Message = exception.Message,
            Errors = new List<ValidationErrorDetail>()
        };

        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Handles domain exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="exception">The domain exception</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        _logger.LogWarning(exception, "Domain rule violation");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new
        {
            Success = false,
            Message = exception.Message,
            Errors = new List<ValidationErrorDetail>()
        };

        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Handles application exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="exception">The application exception</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleApplicationExceptionAsync(HttpContext context, Ambev.DeveloperEvaluation.Domain.Exceptions.AppException exception)
    {
        _logger.LogError(exception, "Application error");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            Success = false,
            Message = exception.Message,
            Errors = new List<ValidationErrorDetail>()
        };

        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Handles unknown exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="exception">The unknown exception</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleUnknownExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = new List<ValidationErrorDetail>()
        };

        await WriteJsonResponseAsync(context, response);
    }

    /// <summary>
    /// Writes a JSON response to the HTTP context
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="response">The response object</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private static async Task WriteJsonResponseAsync(HttpContext context, object response)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}

/// <summary>
/// Extension methods for adding the ExceptionHandlingMiddleware to the application pipeline
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Adds the ExceptionHandlingMiddleware to the application pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
