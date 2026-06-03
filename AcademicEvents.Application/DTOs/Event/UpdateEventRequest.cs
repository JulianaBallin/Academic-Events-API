using System.ComponentModel.DataAnnotations;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Data received to update an existing event.
/// Only the organizer can call this endpoint.
/// </summary>
public class UpdateEventRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must have between 3 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must have between 10 and 2000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Location must have between 3 and 300 characters.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(EventStatus), ErrorMessage = "Invalid status")]
    public EventStatus EventStatus { get; set; }
}
