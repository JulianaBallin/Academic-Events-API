namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Comment made bt user abount an event.
/// </summary>
public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Event? Event { get; set; }
}
