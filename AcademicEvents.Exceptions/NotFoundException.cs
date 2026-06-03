namespace AcademicEvents.Exceptions;

/// <summary>
/// Exception thrown when a resource doesn't exist in database. 
/// they are already registered for.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
