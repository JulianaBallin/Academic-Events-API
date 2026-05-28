using AcademicEvents.Domain.Enums;

namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Inscrição de um usuário em um evento.
/// O índice único no banco impede duplicatas para o mesmo par usuário-evento.
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
