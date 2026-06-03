namespace AcademicEvents.Application.DTOs.Registration;

/// <summary>
/// Registration data returned by read endpoints.
/// </summary>
public class RegistrationResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
