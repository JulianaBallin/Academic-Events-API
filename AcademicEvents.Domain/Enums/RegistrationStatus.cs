namespace AcademicEvents.Domain.Enums;

/// <summary>
/// User registration status for an event.
/// Pending is the initial status; confirmation can be done manually by the organizer.
/// </summary>
public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Canceled
}
