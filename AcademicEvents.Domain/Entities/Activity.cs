using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;


/// <summary>
/// Atividade pertencente a um evento acadêmico.
/// </summary>
public class Activity
{
    public int Id {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public string Descricao {get; set;} = string.Empty;
    public TipoAtividade Tipo {get;set;}
    public DateTime DataInicio {get;set;}
    public DateTime DataFim {get;set;}

    public string Local {get;set;} = string.Empty;

    public int EventId;
    public Event? Event{get;set;}
}