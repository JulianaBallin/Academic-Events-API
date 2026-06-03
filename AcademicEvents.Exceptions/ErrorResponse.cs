namespace AcademicEvents.Exceptions;

/// <summary>
/// Simple template used in API error responses.
/// Helps the client to always receive the same format when a rule fails.
/// </summary>
public class ErrorResponse
{
    public string Message { get; init; } = string.Empty;
    public int StatusCode { get; init; }
    public string Path { get; init; } = string.Empty;
    public DateTime UtcTime { get; init; }
}
