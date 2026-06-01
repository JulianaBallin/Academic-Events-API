using AcademicEvents.Exceptions;

namespace AcademicEvents.API.Middlewares;

/// <summary>
/// Middleware responsável por transformar exceções conhecidas em respostas HTTP padronizadas.
/// Mantém os controllers mais enxutos e facilita a demonstração dos códigos de erro no Swagger.
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

            int statusCode = ObterStatusCode(ex);

            if (statusCode == StatusCodes.Status500InternalServerError)
                _logger.LogError(ex, "Erro inesperado ao processar a requisição.");
            else
                _logger.LogWarning(ex, "Requisição finalizada com erro conhecido.");

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            ErrorResponse response = new ErrorResponse
            {
                Mensagem = statusCode == StatusCodes.Status500InternalServerError
                    ? "Ocorreu um erro interno ao processar a requisição."
                    : ex.Message,
                StatusCode = statusCode,
                Caminho = context.Request.Path,
                DataHoraUtc = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static int ObterStatusCode(Exception exception)
    {
        return exception switch
        {
            DuplicateEmailException => StatusCodes.Status400BadRequest,
            InscricaoDuplicadaException => StatusCodes.Status400BadRequest,
            InvalidCredentialsException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            AcademicEvents.Exceptions.UnauthorizedException => StatusCodes.Status403Forbidden,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
