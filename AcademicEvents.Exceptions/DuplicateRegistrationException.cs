namespace AcademicEvents.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to register for an event
/// they are already registered for.
/// </summary>
public class DuplicateRegistrationException : Exception
{
    public DuplicateRegistrationException(string message) : base(message) { }
}