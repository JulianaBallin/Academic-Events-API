using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Evento academico criado por um usuario organizador.
/// Pode ser publicado, cancelado ou concluido.
/// </summary>
public class Event
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
    public StatusEvento Status { get; set; } = StatusEvento.Rascunho;
    public int OrganizadorId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public User? Organizador { get; set; }
    public List<Registration> Inscricoes { get; set; } = new();
    public List<Comment> Comentarios { get; set; } = new();
    public List<Reaction> Reacoes { get; set; } = new();
}
