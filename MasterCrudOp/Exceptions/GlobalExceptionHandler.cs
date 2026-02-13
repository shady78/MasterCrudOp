using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MasterCrudOp.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception,
            "Unhandled exception occured. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var (statusCode, title) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = GetProblemType(statusCode),
            Instance = httpContext.Request.Path,
            Detail = GetSaferErrorMessage(exception , httpContext)
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        AppException appEx => ((int)appEx.StatusCode , appEx.Message),
        ArgumentNullException => (400 , "Invalid argument Provided"),
        UnauthorizedAccessException  => (401 , "Unauthorized"),
        _ => (500 , "An unexpected error occured")
    };

    private static string GetProblemType(int statusCode) => statusCode switch
    {
        400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
        404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
    };

    private static string? GetSaferErrorMessage(Exception exception , HttpContext context)
    {
        var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();

        if (environment.IsDevelopment())
        {
            return exception.Message;
        }
        
        return exception is AppException ? exception.Message : null;
    }
}
