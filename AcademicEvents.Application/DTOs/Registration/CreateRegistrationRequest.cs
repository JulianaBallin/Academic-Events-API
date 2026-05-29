using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Registration;

/// <summary>
/// Dados recebidos para inscrever o usuário em um evento.
/// O usuário é identificado pelo token JWT, não pelo body.
/// </summary>
public class CreateRegistrationRequest
{
    [Required(ErrorMessage = "O id do evento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O id do evento deve ser maior que zero.")]
    public int EventoId { get; set; }
}
