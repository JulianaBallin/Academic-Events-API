using System.ComponentModel.DataAnnotations;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Reaction;

/// <summary>
/// Data received to react to an event.
/// Each user can react only once per event.
/// </summary>
public class CreateReactionRequest
{
    [Required(ErrorMessage = "Event id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Event id must be greater than zero.")]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Reaction type is required.")]
    [EnumDataType(typeof(ReactionType), ErrorMessage = "Invalid reaction type.")]
    public ReactionType Type { get; set; }
}
