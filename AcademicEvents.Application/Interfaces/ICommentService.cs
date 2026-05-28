using AcademicEvents.Application.DTOs.Comment;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contrato do service de comentários.
/// </summary>
public interface ICommentService
{
    Task<CommentResponse> CreateAsync(CreateCommentRequest request, int usuarioId);
    Task<List<CommentResponse>> GetByEventoAsync(int eventoId);
    Task DeleteAsync(int id, int usuarioId);
}
