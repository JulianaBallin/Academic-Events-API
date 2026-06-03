namespace AcademicEvents.Exceptions;

/// <summary>
/// Exception thrown when attempting to register an email address that already exists.
/// </summary>
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message) : base(message) { }
}
