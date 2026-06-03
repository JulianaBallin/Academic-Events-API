using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Activity;

/// <summary>
/// Activity data returned by read endpoints.
/// </summary>
public class ActivityResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ActivityType Type { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndedAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public int EventId { get; set; }
}
