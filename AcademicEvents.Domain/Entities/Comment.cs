namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Comentário feito por um usuário em um evento.
/// </summary>
public class Comment
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int EventoId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public User? Usuario { get; set; }
    public Event? Evento { get; set; }
}
