namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Comment data returned by read endpoints.
/// </summary>
public class CommentResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
