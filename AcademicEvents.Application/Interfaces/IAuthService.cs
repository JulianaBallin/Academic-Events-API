using AcademicEvents.Application.DTOs.Auth;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract of authentication service.
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
