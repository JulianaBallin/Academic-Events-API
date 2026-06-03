using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Reaction made by a User to an Event.
/// Each User may react with one kind of reaction once by event. 
/// </summary>
public class Reaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public ReactionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Event? Event { get; set; }
}
