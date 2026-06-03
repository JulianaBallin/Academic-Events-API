using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Academic event created by an organizer user
/// Can be published, canceled or finished
/// </summary>
public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndedAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public EventStatus EventStatus { get; set; } = EventStatus.Draft;
    public int OrganizerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Organizer { get; set; }
    public List<Registration> Registrations { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Reaction> Reactions { get; set; } = new();
}
