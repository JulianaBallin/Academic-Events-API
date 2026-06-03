namespace AcademicEvents.Exceptions;

/// <summary>
/// Exception thrown when a user attempts an unauthorized action.
/// Example: edit a Event created by other User.
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}
