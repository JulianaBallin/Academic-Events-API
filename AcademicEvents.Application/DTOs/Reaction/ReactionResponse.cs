namespace AcademicEvents.Application.DTOs.Reaction;

/// <summary>
/// Reaction data returned by read endpoints.
/// </summary>
public class ReactionResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Typo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
