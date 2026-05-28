using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contrato do repository de comentarios.
/// A implementacao fica na Infrastructure.
/// </summary>
public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comentario);
    Task<Comment?> GetByIdAsync(int id);
    Task<List<Comment>> GetByEventoAsync(int eventoId);
    Task DeleteAsync(int id);
}
