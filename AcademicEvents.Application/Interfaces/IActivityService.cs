using AcademicEvents.Application.DTOs.Activity;

namespace AcademicEvents.Application.Interfaces;

/// <summary>
/// Contract for the activity service.
/// </summary>
public interface IActivityService
{
    Task<ActivityResponse> CreateAsync(CreateActivityRequest request, int userId);
    Task<ActivityResponse?> GetByIdAsync(int id);
    Task<List<ActivityResponse>> GetByEventIdAsync(int eventId);
    Task<ActivityResponse> UpdateAsync(int activityId, UpdateActivityRequest request, int userId);
    Task DeleteAsync(int activityId, int userId);
}
