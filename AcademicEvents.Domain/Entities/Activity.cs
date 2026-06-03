using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Activity belonging to an academic event.
/// </summary>
public class Activity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ActivityType Type { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndedAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public int EventId { get; set; }
    public Event? Event { get; set; }
}
