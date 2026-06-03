using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Registration of a User to a Event.
/// The unique index on the database prevents duplicates or the same user-event pair
/// </summary>
public class Registration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Event? Event { get; set; }
}
