namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Dados do comentario retornados nos endpoints de leitura.
/// </summary>
public class CommentResponse
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
