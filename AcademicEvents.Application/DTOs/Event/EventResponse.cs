using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Event data returned by read endpoints.
/// Does not expose internal entity details.
/// </summary>
public class EventResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int OrganizerId { get; set; }
    public string OrganizerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
