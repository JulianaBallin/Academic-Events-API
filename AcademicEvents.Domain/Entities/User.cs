namespace AcademicEvents.Domain.Entities;

/// <summary>
/// Representa um usuario cadastrado na plataforma.
/// Pode organizar eventos, se inscrever, comentar e reagir.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // senha nunca fica aqui em texto puro, sempre o hash
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<Event> EventosOrganizados { get; set; } = new();
    public List<Registration> Inscricoes { get; set; } = new();
    public List<Comment> Comentarios { get; set; } = new();
    public List<Reaction> Reacoes { get; set; } = new();
}
