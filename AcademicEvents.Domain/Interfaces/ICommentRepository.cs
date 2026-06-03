using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Comments repository contract.
/// Implementation kept on Infrastructure.
/// </summary>
public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment?> GetByIdAsync(int id);
    Task<List<Comment>> GetByEventAsync(int eventId);
    Task DeleteAsync(int id);
}
