using System.ComponentModel.DataAnnotations;
using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Reaction;

/// <summary>
/// Dados recebidos para reagir a um evento.
/// Cada usuário pode reagir apenas uma vez por evento.
/// </summary>
public class CreateReactionRequest
{
    [Required(ErrorMessage = "O id do evento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O id do evento deve ser maior que zero.")]
    public int EventoId { get; set; }

    [Required(ErrorMessage = "O tipo de reação é obrigatório.")]
    public TipoReacao Tipo { get; set; }
}
