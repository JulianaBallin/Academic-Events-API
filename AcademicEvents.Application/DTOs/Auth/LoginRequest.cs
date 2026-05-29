using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Auth;

/// <summary>
/// Dados recebidos no endpoint de login.
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;
}
