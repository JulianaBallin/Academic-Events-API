using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Dados recebidos para atualizar um evento existente.
/// </summary>
public class UpdateEventRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
    public StatusEvento Status { get; set; }
}
