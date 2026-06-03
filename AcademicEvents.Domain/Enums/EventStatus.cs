namespace AcademicEvents.Domain.Enums;

/// <summary>
/// Life cycle of a Academic Event.
/// Draft is the initial state when creating an event; Published makes the event visible for registration.
/// </summary>
public enum EventStatus
{
    Draft,
    Published,
    Canceled,
    Finished
}
