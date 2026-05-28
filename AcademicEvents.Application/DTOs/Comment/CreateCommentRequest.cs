namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Dados recebidos para criar um comentario em um evento.
/// </summary>
public class CreateCommentRequest
{
    public int EventoId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
}
