namespace AcademicEvents.Exceptions;

/// <summary>
/// Exception thrown when login credentials are invalid.
/// Does not specify whether the email or password is incorrect for security reasons.
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message) { }
}
