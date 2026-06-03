using AcademicEvents.Exceptions;

namespace AcademicEvents.API.Middlewares;

/// <summary>
/// Converts known exceptions into standardized HTTP responses.
/// Keeps controllers cleaner and improves error response documentation in Swagger.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
                throw;

            int statusCode = GetStatusCode(ex);

            
            if (statusCode == StatusCodes.Status500InternalServerError)
                _logger.LogError(ex, "Unexpected error while processing the request.");
            else
                _logger.LogWarning(ex, "Request completed with a known error.");

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            ErrorResponse response = new ErrorResponse
            {
                Message = statusCode == StatusCodes.Status500InternalServerError
                    ? "An unexpected error occurred while processing the request."
                    : ex.Message,
                StatusCode = statusCode,
                Path = context.Request.Path,
                UtcTime = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            DuplicateEmailException => StatusCodes.Status400BadRequest,
            DuplicateRegistrationException => StatusCodes.Status400BadRequest,
            InvalidCredentialsException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            AcademicEvents.Exceptions.UnauthorizedException => StatusCodes.Status403Forbidden,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
