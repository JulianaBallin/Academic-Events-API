using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Registrations repository contract.
/// Implementation kept on Infrastructure.
/// </summary>
public interface IRegistrationRepository
{
    Task<Registration> CreateAsync(Registration registration);
    Task<Registration?> GetByIdAsync(int id);
    Task<List<Registration>> GetByUserAsync(int userId);
    Task<List<Registration>> GetByEventAsync(int eventId);
    // validates whether the user-event pair exists before insert
    Task<Registration?> GetByUserEventAsync(int userId, int eventId);
    Task<Registration?> UpdateAsync(Registration registration);
    Task DeleteAsync(int id);
}
