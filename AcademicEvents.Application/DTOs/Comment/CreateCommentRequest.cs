using System.ComponentModel.DataAnnotations;

namespace AcademicEvents.Application.DTOs.Comment;

/// <summary>
/// Dados recebidos para criar um comentário em um evento.
/// </summary>
public class CreateCommentRequest
{
    [Required(ErrorMessage = "O id do evento é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O id do evento deve ser maior que zero.")]
    public int EventoId { get; set; }

    [Required(ErrorMessage = "O conteúdo do comentário é obrigatório.")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "O comentário deve ter entre 1 e 1000 caracteres.")]
    public string Conteudo { get; set; } = string.Empty;
}
