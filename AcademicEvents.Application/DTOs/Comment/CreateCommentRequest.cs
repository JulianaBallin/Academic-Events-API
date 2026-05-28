namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Dados recebidos para criar um comentário em um evento.
/// </summary>
public class CreateCommentRequest
{
    public int EventoId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
}
