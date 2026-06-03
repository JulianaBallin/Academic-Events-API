using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Application.DTOs.Activity;

/// <summary>
/// Dados de atividade retornados nos endpoints de leitura.
/// Não expõe detalhes internos da entidade.
/// </summary>
public class ActivityResponse
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public TipoAtividade Tipo { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public string Local { get; set; } = string.Empty;

    public int EventId { get; set; }
}