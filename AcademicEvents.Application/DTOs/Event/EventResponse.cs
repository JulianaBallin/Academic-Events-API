using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Event;

/// <summary>
/// Dados do evento retornados nos endpoints de leitura.
/// Não expõe detalhes internos da entidade.
/// </summary>
public class EventResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int OrganizadorId { get; set; }
    public string NomeOrganizador { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
