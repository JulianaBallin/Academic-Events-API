namespace AcademicEvents.Application.DTOs.Auth;

/// <summary>
/// Resposta dos endpoints de autenticação.
/// Contém o token JWT e dados básicos do usuário.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
}
