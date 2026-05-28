using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Reação de um usuário a um evento.
/// Cada usuário pode reagir uma vez por evento com um tipo diferente.
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
