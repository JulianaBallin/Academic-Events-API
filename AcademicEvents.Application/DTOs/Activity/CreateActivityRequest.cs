using AcademicEvents.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Activity;

/// <summary>
/// Data received at the activity creation endpoint.
/// </summary>
public class CreateActivityRequest
{
    [Required(ErrorMessage = "Event id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Event id must be greater than zero.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must have between 3 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must have between 10 and 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Activity type is required.")]
    [EnumDataType(typeof(ActivityType), ErrorMessage = "Invalid activity type.")]
    public ActivityType Type { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartAt { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateTime? EndedAt { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Location must have between 3 and 300 characters.")]
    public string Location { get; set; } = string.Empty;
}
