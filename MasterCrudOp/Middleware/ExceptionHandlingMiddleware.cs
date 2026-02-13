using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterCrudOp.Middleware;
// Custom Middleware 
public class ExceptionHandlingMiddleware(RequestDelegate next , ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unhandled exception Occured");
			await HandleExceptionAsync(context, ex);
		}
    }

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		context.Response.StatusCode = exception switch
		{
			KeyNotFoundException => StatusCodes.Status404NotFound,
			ArgumentException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};

		var problemDetails = new ProblemDetails
		{
			Status = context.Response.StatusCode,
			Title = "An error occurred",
			Detail = exception.Message
		};

		await context.Response.WriteAsJsonAsync(problemDetails);
	}
}
