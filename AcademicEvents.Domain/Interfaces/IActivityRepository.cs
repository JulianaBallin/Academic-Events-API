using AcademicEvents.Domain.Entities;

namespace AcademicEvents.Domain.Interfaces;

/// <summary>
/// Contract for the activity repository.
/// Implementation lives in Infrastructure.
/// </summary>
public interface IActivityRepository
{
    Task<Activity> CreateAsync(Activity activity);
    Task<Activity?> GetByIdAsync(int id);
    Task<List<Activity>> GetByEventIdAsync(int eventId);
    Task UpdateAsync(Activity activity);
    Task DeleteAsync(Activity activity);
}
