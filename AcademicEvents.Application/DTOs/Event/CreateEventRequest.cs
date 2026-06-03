using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Event data returned by read endpoints.
/// Does not expose internal entity details.
/// </summary>
public class CreateEventRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must have between 3 e 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must have between 10 e 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Location must have between 3 e 300 characters.")]
    public string Location { get; set; } = string.Empty;
}
