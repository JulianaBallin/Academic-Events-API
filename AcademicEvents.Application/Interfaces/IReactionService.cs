using AcademicEvents.Application.DTOs.Reaction;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract of event reaction service.
/// </summary>
public interface IReactionService
{
    Task<ReactionResponse> CreateAsync(CreateReactionRequest request, int userId);
    Task<List<ReactionResponse>> GetByEventAsync(int eventId);
    Task DeleteAsync(int id, int userId);
}
