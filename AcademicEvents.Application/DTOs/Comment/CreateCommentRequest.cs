using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Data received to create a comment on an event.
/// </summary>
public class CreateCommentRequest
{
    [Required(ErrorMessage = "Event ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Event id must be greater than zero.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment must have between 1 and 1000 characters.")]
    public string Content { get; set; } = string.Empty;
}
