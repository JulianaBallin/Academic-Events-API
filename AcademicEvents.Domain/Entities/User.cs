namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Represents a registered user on the platform.
/// Can organize events, register, comment, and react.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // Password never kept in pure text,  always on hash.
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Event> OrganizedEventList { get; set; } = new();
    public List<Registration> Registrations { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<Reaction> Reactions { get; set; } = new();
}
