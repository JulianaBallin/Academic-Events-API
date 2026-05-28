namespace AcademicEvents.Application.DTOs.Auth;

/// <summary>
/// Dados recebidos no endpoint de login.
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
