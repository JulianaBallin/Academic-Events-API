using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Reaction;

/// <summary>
/// Dados recebidos para reagir a um evento.
/// </summary>
public class CreateReactionRequest
{
    public int EventoId { get; set; }
    public TipoReacao Tipo { get; set; }
}
