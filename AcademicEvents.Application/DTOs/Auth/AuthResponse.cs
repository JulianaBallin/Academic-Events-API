namespace AcademicEvents.Application.DTOs.Auth;

/// <summary>
/// Authentication endpoint responses
/// Contains the JWT Token and basic user date.
/// </summary>
public class AuthResponse
{
    public string Token { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime ExpiresIn { get; set; }
}
