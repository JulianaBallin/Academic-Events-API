using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Inscricao de um usuario em um evento.
/// O indice unico no banco impede duplicatas para o mesmo par usuario-evento.
/// </summary>
public class Registration
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int EventoId { get; set; }
    public StatusInscricao Status { get; set; } = StatusInscricao.Pendente;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public User? Usuario { get; set; }
    public Event? Evento { get; set; }
}
