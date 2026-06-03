using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Registration;

/// <summary>
/// Data received to register the user for an event.
/// The user is identified by the JWT token, not by the request body.
/// </summary>
public class CreateRegistrationRequest
{
    [Required(ErrorMessage = "Event id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Event id must be greater than zero.")]
    public int EventId { get; init; }
}
