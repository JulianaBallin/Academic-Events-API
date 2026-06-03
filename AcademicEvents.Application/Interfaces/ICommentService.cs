using AcademicEvents.Application.DTOs.Comment;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract of comments service.
/// </summary>
public interface ICommentService
{
    Task<CommentResponse> CreateAsync(CreateCommentRequest request, int userId);
    Task<List<CommentResponse>> GetByEventAsync(int eventId);
    Task DeleteAsync(int id, int userId);
}
