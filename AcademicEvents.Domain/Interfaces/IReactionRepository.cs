using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Reaction repository contract.
/// Implementation kept on Infrastructure.
/// </summary>
public interface IReactionRepository
{
    Task<Reaction> CreateAsync(Reaction reaction);
    Task<Reaction?> GetByIdAsync(int id);
    Task<List<Reaction>> GetByEventAsync(int eventId);
    Task<Reaction?> GetByUserEventAsync(int userId, int eventId);
    Task DeleteAsync(int id);
}
