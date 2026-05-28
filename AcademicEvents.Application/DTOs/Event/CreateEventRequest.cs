namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Dados recebidos para criar um novo evento.
/// </summary>
public class CreateEventRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
}
