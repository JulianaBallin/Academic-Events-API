using AcademicEvents.Application.DTOs.Auth;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service de autenticacao.
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
