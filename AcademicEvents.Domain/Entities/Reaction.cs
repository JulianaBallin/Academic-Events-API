using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Reacao de um usuario a um evento.
/// Cada usuario pode reagir uma vez por evento com um tipo diferente.
/// </summary>
public class Reaction
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int EventoId { get; set; }
    public TipoReacao Tipo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public User? Usuario { get; set; }
    public Event? Evento { get; set; }
}
